using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Enemy Reference Variables
    public float moveSpeed = 5f;
    public float destroyThresholdX = -10f;

    // Raycast Reference Variables
    public float detectionRadius = 5f;
    public float detectionAngle = 90f;
    public int rayNumbers = 20;
    public LayerMask targetMask;

    // Chase Reference Variables
    public float stopChaseDistance = 4f;
    public float straightMoveDistance = 5f;
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
                Debug.Log($"¡Jugador detectado dentro del cono de visión! Objeto: {hit.collider.name}");
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
            Debug.Log("Jugador muy cerca. Moviéndose en línea recta hacia la izquierda.");
            moveInStraightLine = true;
            chaseDirection = -transform.right; 
        }

        transform.position += chaseDirection * moveSpeed * Time.deltaTime;

        if (distanceToPlayer > detectionRadius * 1.5f)
        {
            Debug.Log("Jugador fuera de rango. Enemigo vuelve a la búsqueda...");
            playerTarget = null;
            moveInStraightLine = false;
        }
    }
}
