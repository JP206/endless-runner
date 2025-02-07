using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerSpacialDetector playerSpacialDetector;
    Animator animator;

    [SerializeField] float gravityStrength = 3f;
    [SerializeField] float ceilingDampingFactor = 0.3f; // Factor para suavizar la detención del salto

    float yVelocity;
    float deltaY => yVelocity * Time.fixedDeltaTime;

    public bool isJumping => yVelocity > 0;

    public void InitializeReferences(PlayerSpacialDetector playerSpacialDetector, Animator animator)
    {
        this.playerSpacialDetector = playerSpacialDetector;
        this.animator = animator;
    }

    public void MovementY()
    {
        animator.SetFloat("ySpeed", yVelocity);

        // 🔹 DETECCIÓN DE TECHO
        if (yVelocity > 0 && playerSpacialDetector.IsCeilingDetected(0.1f))
        {
            // Reducimos la velocidad para no cortar el salto bruscamente
            yVelocity *= ceilingDampingFactor;

            // Ajustamos la posición para que NO atraviese el techo
            float ceilingY = playerSpacialDetector.CeilingYPosition();
            if (ceilingY != float.PositiveInfinity)
            {
                transform.position = new Vector3(transform.position.x, ceilingY);
                yVelocity = 0; // Detenemos la velocidad para evitar que siga subiendo
            }
        }

        // 🔹 SI NO ESTÁ EN EL SUELO, APLICA GRAVEDAD
        if (!playerSpacialDetector.IsGrounded(0.2f, deltaY))
        {
            yVelocity -= gravityStrength;

            if (yVelocity < 0)
            {
                animator.SetBool("isFalling", true);
            }
        }

        // 🔹 SI TOCA EL SUELO, RESETEAR VELOCIDAD Y POSICIÓN
        if (!isJumping && playerSpacialDetector.IsGrounded(0.1f, deltaY))
        {
            yVelocity = 0;
            Vector2 newPos = new Vector2(transform.position.x, playerSpacialDetector.GroundYPosition());
            transform.position = newPos;
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }

        // 🔹 ACTUALIZAR POSICIÓN SOLO SI NO ATRAVIESA EL TECHO
        if (!playerSpacialDetector.IsCeilingDetected(0.1f) || yVelocity < 0)
        {
            transform.position += new Vector3(0, deltaY, 0);
        }
    }

    public void ApplyJump(float jumpSpeed)
    {
        if (playerSpacialDetector.IsGrounded(0.1f, deltaY))
        {
            yVelocity = jumpSpeed;
            animator.SetBool("isJumping", true);
        }
    }
}
