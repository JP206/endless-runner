using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerSpacialDetector playerSpacialDetector;
    Animator animator;

    [SerializeField] float gravityStrength;
    [SerializeField] float wallDetectionDistance = 0.1f;

    float yVelocity;
    bool detectedWall = false;

    // Props
    float deltaY => yVelocity * Time.fixedDeltaTime;
    public bool isJumping => yVelocity > 0;

    public void InitializeReferences(PlayerSpacialDetector playerSpacialDetector, Animator animator)
    {
        this.playerSpacialDetector = playerSpacialDetector;
        this.animator = animator;
    }

    void FixedUpdate()
    {
        if (!detectedWall) DetectWall();
        MovementY();
    }

    public void MovementY()
    {
        animator.SetFloat("ySpeed", yVelocity);

        if (!playerSpacialDetector.IsGrounded(0.2f, deltaY))
        {
            yVelocity -= gravityStrength;
            animator.SetBool("isFalling", yVelocity < 0);
        }

        if (playerSpacialDetector.IsRooftop(0.1f, deltaY, out float ceilingY) && yVelocity > 0)
        {
            yVelocity = Mathf.Max(yVelocity - gravityStrength * 2f, 0);

            float maxY = ceilingY - (playerSpacialDetector.GetActorHeight() / 2f);
            transform.position = (transform.position.y > maxY)
                ? new Vector2(transform.position.x, maxY)
                : transform.position;
        }

        if (!isJumping && playerSpacialDetector.IsGrounded(0.1f, deltaY))
        {
            yVelocity = 0;
            transform.position = new Vector2(transform.position.x, playerSpacialDetector.GroundYPosition());
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }

        transform.position += new Vector3(0, deltaY, 0);
    }

    public void ApplyJump(float jumpSpeed)
    {
        yVelocity = jumpSpeed;
        detectedWall = false;
    }

    private void DetectWall()
    {
        bool isWallDetected = playerSpacialDetector.HasWall();
        detectedWall = (isWallDetected && !detectedWall) ? true : detectedWall;
    }

}
