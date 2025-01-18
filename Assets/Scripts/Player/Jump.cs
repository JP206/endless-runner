using UnityEngine;

public class Jump : MonoBehaviour
{
    PlayerSpacialDetector playerSpacialDetector;
    PlayerMovement playerMovement;
    Animator animator;

    [SerializeField] float jumpStrength;

    public void InitializeReferences(PlayerSpacialDetector playerSpacialDetector, PlayerMovement playerMovement, Animator animator)
    {
        this.playerSpacialDetector = playerSpacialDetector;
        this.playerMovement = playerMovement;
        this.animator = animator;
    }

    public void OnJump()
    {
        if (playerSpacialDetector.IsGrounded(0.1f, 0))
        {
            playerMovement.ApplyJump(jumpStrength);
        }
    }
}
