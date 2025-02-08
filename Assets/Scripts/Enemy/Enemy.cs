using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5f, attackThreshold;
    public int damage = 1;
    public LayerMask targetMask;
    public float damageCooldown = 2f;

    private Vector2 targetPosition;
    private bool isAttacking = false;
    private bool hasReachedTarget = false;
    private bool canDealDamage = true;
    private bool attackAnimFlag = false;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isAttacking && !hasReachedTarget)
        {
            MoveTowardsTarget();
        }
        else
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
        Collider2D detectedPlayer = Physics2D.OverlapCircle(transform.position, 10f, targetMask);
        if (detectedPlayer != null && detectedPlayer.CompareTag("Player"))
        {
            targetPosition = detectedPlayer.transform.position;
            isAttacking = true;
        }
    }

    private void MoveTowardsTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        float distanceToPlayer = Vector2.Distance(transform.position, targetPosition);

        if (distanceToPlayer <= 0.1f)
        {
            hasReachedTarget = true;
        }

        if (distanceToPlayer <= attackThreshold && !attackAnimFlag)
        {
            animator.SetTrigger("Attack");
            attackAnimFlag = true;
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
        Gizmos.DrawWireSphere(transform.position, 5f);
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
