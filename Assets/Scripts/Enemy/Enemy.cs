using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Object Movement
    public float moveSpeed = 5f;
    public float destroyThresholdX = -10f; 

    //Raycast Detection Variables
    public float detectionRadius = 5f;  
    public float detectionAngle = 90f;  
    public int rayNumbers = 20;  
    public LayerMask targetMask; 

    private void Start()
    {
        int layerIndex = gameObject.layer;
    }

    private void Update()
    {
        MoveEnemy();
        FunnelDetection();  
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

            if (hit.collider != null)
            {
                Debug.Log($"¡Jugador detectado dentro del cono de visión! Objeto: {hit.collider.name}");
            }
        }
    }
}
