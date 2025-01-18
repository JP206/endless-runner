using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerSpacialDetector playerSpacialDetector;
    Animator animator;

    [SerializeField] float gravityStrength;
    float yVelocity;

    // Props
    float deltaY => yVelocity * Time.fixedDeltaTime;
    bool isJumping => yVelocity > 0;

    public void InitializeReferences(PlayerSpacialDetector playerSpacialDetector, Animator animator)
    {
        this.playerSpacialDetector = playerSpacialDetector;
        this.animator = animator;
    }

    public void MovementY()
    {
        animator.SetFloat("ySpeed", yVelocity);

        if (!playerSpacialDetector.IsGrounded(0.2f, deltaY))
        {
            yVelocity -= gravityStrength;
        }
        
        if (!isJumping && playerSpacialDetector.IsGrounded(0.1f, deltaY))
        {
            yVelocity = 0;
            Vector2 vector2 = new Vector2(transform.position.x, playerSpacialDetector.GroundYPosition());
            transform.position = vector2;
        }
        transform.position += new Vector3(0, deltaY, 0);
    }


    public void ApplyJump(float jumpSpeed)
    {
        yVelocity = jumpSpeed;
    }
}
