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
}
