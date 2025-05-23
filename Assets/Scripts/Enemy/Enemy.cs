using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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
    private bool isVulnerable = true;

    private Animator animator;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private Collider2D enemyCollider;
    private Rigidbody2D rb;
    private ScoreManager scoreManager;

    [SerializeField] AudioSource deathSource;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyCollider = GetComponent<Collider2D>();
        scoreManager = FindAnyObjectByType<ScoreManager>();

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
        if (collision.CompareTag("Player") && canDealDamage && isVulnerable)
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

        if (scoreManager != null) scoreManager.AddScore(5);
        PlayDeathSound();
    }

    public void PlayAttackSound()
    {
        if (audioSource != null && audioSource.clip != null)
            audioSource.PlayOneShot(audioSource.clip);
    }

    public void PlayDeathSound()
    {
        if (deathSource != null && deathSource.clip != null)
            deathSource.PlayOneShot(deathSource.clip);
    }

    public void SetVulnerability(bool state)
    {
        isVulnerable = state;
    }
}
