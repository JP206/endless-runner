using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float attackSpeed = 12f;
    public float attackThreshold = 1f;
    public float detectionRange = 5f;
    public LayerMask targetMask;
    public float damageCooldown = 2f;
    public float fallSpeed = 2f;
    public float blinkDuration = 1f;
    public float blinkInterval = 0.1f;

    private Vector2 targetPosition;
    private bool isAttacking = false;
    private bool hasReachedTarget = false;
    private bool canDealDamage = true;
    private bool isCharging = false;
    private bool isStopped = false;
    private bool isDead = false;

    private Animator animator;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Collider2D enemyCollider;
    private Rigidbody2D rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<Collider2D>();

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0;
    }

    private void Update()
    {
        if (isDead) return;

        if (isCharging) ChargeAttack();
        else if (!isStopped) MoveForward();

        if (!isAttacking) DetectPlayer();
    }

    private void DetectPlayer()
    {
        Collider2D detectedPlayer = Physics2D.OverlapCircle(transform.position, detectionRange, targetMask);
        if (detectedPlayer != null && detectedPlayer.CompareTag("Player"))
        {
            targetPosition = detectedPlayer.transform.position;
            isStopped = true;
            animator.SetTrigger("Attack");
            isAttacking = true;
            PlayAttackSound();
        }
    }

    public void StartCharge()
    {
        isCharging = true;
        isStopped = false;
    }

    private void ChargeAttack()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, attackSpeed * Time.deltaTime);
        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
        if (distanceToTarget <= 0.2f)
        {
            isCharging = false;
            hasReachedTarget = true;
        }
    }

    private void MoveForward()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canDealDamage)
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage();
                StartCoroutine(DamageCooldown());
            }
        }
    }

    private IEnumerator DamageCooldown()
    {
        canDealDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDealDamage = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    public void DestroyEnemy()
    {
        if (isDead) return;
        isDead = true;

        isCharging = false;
        isStopped = true;
        isAttacking = false;

        if (enemyCollider != null) enemyCollider.enabled = false;

        animator.SetTrigger("Death");

        rb.gravityScale = 1;
        rb.linearVelocity = new Vector2(0, -fallSpeed);

        StartCoroutine(BlinkWhileDying());
    }
    private IEnumerator BlinkWhileDying()
    {
        float deathAnimTime = animator.GetCurrentAnimatorStateInfo(0).length;
        float elapsedTime = 0f;

        while (elapsedTime < deathAnimTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsedTime += blinkInterval;
        }

        Destroy(gameObject);
    }

    public void PlayAttackSound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}
