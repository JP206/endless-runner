using System.Collections;
using UnityEngine;

public class PlayerSpacialDetector : MonoBehaviour
{
    public LayerMask detectionLayerMask;
    public float anchoActor = 1f;
    public float detectionInterval = 0.05f;
    public float lookAhead = 5f;

    private void Start()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            anchoActor = boxCollider.bounds.extents.x; 
        }
        else
        {
            anchoActor = 0.5f; 
        }

        StartCoroutine(DetectObstacleRoutine());
    }

    private IEnumerator DetectObstacleRoutine()
    {
        while (true) 
        {
            DetectObstacle();
            yield return new WaitForSeconds(detectionInterval);
        }
    }

    private void DetectObstacle()
    {
        Vector2 rayOrigin = new Vector2(transform.position.x + anchoActor, transform.position.y); 
        Vector2 rayDirection = Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, lookAhead, detectionLayerMask); 
        Debug.DrawRay(rayOrigin, rayDirection * lookAhead, Color.blue, detectionInterval);

        if (hit.collider != null)
        {
            Debug.Log($"¡Enemigo detectado: {hit.collider.name} en la capa {LayerMask.LayerToName(hit.collider.gameObject.layer)}!");
        }
    }
}
