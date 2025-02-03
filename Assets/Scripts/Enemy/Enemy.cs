using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float destroyThresholdX = -10f;

    public float detectionRadius = 5f;
    public float detectionAngle = 90f;
    public int rayNumbers = 20;
    public LayerMask targetMask;

    public float stopChaseDistance = 4f;
    public float straightMoveDistance = 5f;

    public int damage = 1; // Cantidad de da�o que inflige el enemigo

    private Transform playerTarget = null;
    private Vector3 chaseDirection;
    private bool moveInStraightLine = false;

    private void Update()
    {
        if (playerTarget != null && !moveInStraightLine)
        {
            ChasePlayer();
        }
        else
        {
            MoveEnemy();
            FunnelDetection();
        }
    }

    private void MoveEnemy()
    {
        transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);

        if (transform.position.x < destroyThresholdX)
        {
            Destroy(gameObject);
        }
    }

    private void FunnelDetection()
    {
        float halfAngle = detectionAngle / 2f;
        float angleStep = detectionAngle / (rayNumbers - 1);

        for (int i = 0; i < rayNumbers; i++)
        {
            float currentAngle = -halfAngle + (angleStep * i);
            Vector2 rayDirection = Quaternion.Euler(0, 0, currentAngle) * -transform.right;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, detectionRadius, targetMask);
            Debug.DrawRay(transform.position, rayDirection * detectionRadius, Color.green);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                playerTarget = hit.collider.transform;
                chaseDirection = (playerTarget.position - transform.position).normalized;
                break;
            }
        }
    }

    private void ChasePlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);

        if (distanceToPlayer > stopChaseDistance)
        {
            chaseDirection = (playerTarget.position - transform.position).normalized;
        }

        if (distanceToPlayer <= straightMoveDistance)
        {
            moveInStraightLine = true;
            chaseDirection = -transform.right;
        }

        transform.position += chaseDirection * moveSpeed * Time.deltaTime;

        if (distanceToPlayer <= 1f) // Si el enemigo est� muy cerca, inflige da�o
        {
            DealDamage();
        }

        if (distanceToPlayer > detectionRadius * 1.5f)
        {
            playerTarget = null;
            moveInStraightLine = false;
        }
    }

    private void DealDamage()
    {
        PlayerHealth playerHealth = playerTarget.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage();
            Debug.Log("�El enemigo ha infligido da�o al jugador!");
        }
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
