using UnityEngine;

public class GroundedEnemy : MonoBehaviour
{
    [Header("Detección")]
    public float detectionRange = 3f;
    public LayerMask playerLayer;
    public Vector2 detectionOffset = Vector2.zero;

    private Animator animator;
    private bool playerWasInRangeLastFrame = false;
    private bool isDead = false;
    private Rigidbody2D rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isDead) return;

        DetectPlayerEntry();
    }

    private void DetectPlayerEntry()
    {
        Vector2 center = (Vector2)transform.position + detectionOffset;
        Collider2D player = Physics2D.OverlapCircle(center, detectionRange, playerLayer);

        bool isPlayerInRange = (player != null);

        if (isPlayerInRange && !playerWasInRangeLastFrame)
        {
            animator.SetTrigger("Attack");
        }

        playerWasInRangeLastFrame = isPlayerInRange;
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        animator.SetTrigger("Death");

        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 3;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.linearVelocity = Vector2.zero;
        }

        Invoke(nameof(DeactivateSelf), 1f);
    }

    private void DeactivateSelf()
    {
        gameObject.SetActive(false);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 center = (Vector2)transform.position + detectionOffset;
        Gizmos.DrawWireSphere(center, detectionRange);
    }

    public void IgnoreCollisionWithPlayer(bool ignore)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Collider2D[] enemyColliders = GetComponentsInChildren<Collider2D>();
        Collider2D[] playerColliders = player.GetComponentsInChildren<Collider2D>();

        foreach (var enemyCol in enemyColliders)
        {
            foreach (var playerCol in playerColliders)
            {
                Physics2D.IgnoreCollision(enemyCol, playerCol, ignore);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null && !player.IsDead())
            {
                player.TakeDamage();
            }
        }
    }

}
