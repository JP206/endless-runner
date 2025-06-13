using UnityEngine;

public class FallingRockDmg : MonoBehaviour
{
    [SerializeField] float damageAmount = 40f;

    private Rigidbody2D rb;
    private bool hasFallen = false;
    private bool hasHitPlayer = false;
    private bool hasTouchedGround = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasFallen) return;

        if (other.CompareTag("Player"))
        {
            Transform triggerZone = transform.Find("TriggerZone");
            if (triggerZone != null)
                triggerZone.gameObject.SetActive(false);

            TriggerFall();
        }
    }

    private void TriggerFall()
    {
        hasFallen = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 3f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasHitPlayer && collision.collider.CompareTag("Player"))
        {
            PlayerHealth player = collision.collider.GetComponent<PlayerHealth>();
            if (player != null && !player.IsDead())
            {
                player.TakeDamage();
                hasHitPlayer = true;
            }
        }

        if (collision.collider.CompareTag("Floor") && !hasTouchedGround)
        {
            hasTouchedGround = true;
            gameObject.SetActive(false);
        }
    }

    public void ResetState()
    {
        hasFallen = false;
        hasHitPlayer = false;
        hasTouchedGround = false;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        Transform triggerZone = transform.Find("TriggerZone");
        if (triggerZone != null)
            triggerZone.gameObject.SetActive(true);

        gameObject.SetActive(true);
    }
}
