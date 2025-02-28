using System.Collections;
using UnityEngine;

public class PlayerSpacialDetector : MonoBehaviour
{
    [SerializeField] LayerMask detectionLayerMask;
    [SerializeField] float actorWidth;
    [SerializeField] float actorHeight;

    public bool IsGrounded(float minLookAhead, float lookAhead)
    {
        var totalLookAhead = Mathf.Abs(lookAhead) + minLookAhead;
        Vector3 origin = transform.position + new Vector3(0, actorHeight / 2);

        Debug.DrawRay(origin, Vector2.down * (actorHeight + totalLookAhead), Color.black, 0.1f);
        return Physics2D.Raycast(origin, Vector2.down, actorHeight + totalLookAhead, detectionLayerMask);
    }

    public bool IsRooftop(float minLookAhead, float lookAhead, out float ceilingY)
    {
        float totalLookAhead = Mathf.Abs(lookAhead) + minLookAhead;

        Vector2 rayOrigin = transform.position;
        Vector2 rayDirection = Vector2.up;

        float rayDistance = actorHeight / 2f + totalLookAhead;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, detectionLayerMask);

        Color rayColor = hit.collider != null ? Color.red : Color.blue;
        Debug.DrawRay(rayOrigin, rayDirection * rayDistance, rayColor);

        if (hit.collider != null)
        {
            ceilingY = hit.point.y;
            return true;
        }

        ceilingY = 0;
        return false;
    }

    public bool HasWall()
    {
        Vector2 rayOrigin = transform.position;
        Vector2 rayDirection = Vector2.right;

        float rayDistance = actorWidth / 2f + 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, detectionLayerMask);

        Color rayColor = hit.collider != null ? Color.red : Color.green;
        Debug.DrawRay(rayOrigin, rayDirection * rayDistance, rayColor);

        return hit.collider != null;
    }

    public float GroundYPosition()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 100f, detectionLayerMask);
        
        if (hit) return hit.point.y + (actorHeight / 2f);

        return transform.position.y;
    }

    public float GetActorHeight() { return actorHeight; }
}
