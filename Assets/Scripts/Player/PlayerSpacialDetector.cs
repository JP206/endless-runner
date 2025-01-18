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

        Debug.DrawRay(transform.position + new Vector3(0, actorHeight / 2), Vector2.down * (actorHeight + totalLookAhead), Color.black, 0.1f);

        return Physics2D.Raycast(transform.position + new Vector3(0, actorHeight / 2), Vector2.down, actorHeight + totalLookAhead, detectionLayerMask);
    }

    public float GroundYPosition()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 100f, detectionLayerMask).point.y + (actorHeight / 2f);
    }

    public Enemy DetectEnemy(float direction, float lookAhead)
    {
        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - (actorHeight / 2f));
        Vector2 rayDirection = Vector2.right * direction;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, actorWidth / 2f + lookAhead, detectionLayerMask);
        Debug.DrawRay(rayOrigin, (rayDirection * lookAhead), Color.blue, 1);

        if (hit.collider != null)
        {
            return hit.collider.GetComponent<Enemy>();
        }
        else { return null; }
    }
}
