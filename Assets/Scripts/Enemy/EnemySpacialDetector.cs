using UnityEngine;

public class EnemySpacialDetector : MonoBehaviour
{
    public float detectionRadius = 5f;
    public float detectionAngle = 90f;
    public int rayNumbers = 20;
    public LayerMask targetMask;

    private void Update()
    {
        FunnelDetection();
    }

    private void FunnelDetection()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, detectionRadius, targetMask);
        Debug.DrawRay(transform.position, transform.right * detectionRadius, Color.green);

        if (hit.collider != null) { }
    }
}
