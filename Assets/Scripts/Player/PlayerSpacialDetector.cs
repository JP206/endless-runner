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

    public float GroundYPosition()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 100f, detectionLayerMask);
        if (hit)
        {
            return hit.point.y + (actorHeight / 2f);
        }
        return transform.position.y;
    }

    public bool IsCeilingDetected(float extraDistance)
    {
        Vector2 origin = (Vector2)transform.position + new Vector2(0, actorHeight / 2);
        Debug.DrawRay(origin, Vector2.up * extraDistance, Color.red, 0.1f);
        return Physics2D.Raycast(origin, Vector2.up, extraDistance, detectionLayerMask);
    }


    public float CeilingYPosition()
    {
        Vector2 origin = transform.position + new Vector3(0, actorHeight / 2);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.up, 100f, detectionLayerMask);
        if (hit)
        {
            return hit.point.y - (actorHeight / 2f);
        }
        return float.PositiveInfinity;
    }
}
