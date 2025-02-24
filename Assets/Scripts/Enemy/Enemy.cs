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

    private Vector2 targetPosition;
    private bool isAttacking = false;
    private bool hasReachedTarget = false;
    private bool canDealDamage = true;
    private bool isCharging = false;
    private bool isStopped = false;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isCharging)
        {
            ChargeAttack();
        }
        else if (!isStopped)
        {
            MoveForward();
        }

        if (!isAttacking)
        {
            DetectPlayer();
        }
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
        Destroy(gameObject);
    }
}
