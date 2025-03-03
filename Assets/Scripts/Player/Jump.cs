using UnityEngine;

public class Jump : MonoBehaviour
{
    PlayerSpacialDetector playerSpacialDetector;
    PlayerMovement playerMovement;
    Animator animator;
    AudioSource audioSource;
    AudioClip jumpSound;

    [SerializeField] float jumpStrength;

    public void InitializeReferences(
        PlayerSpacialDetector playerSpacialDetector,
        PlayerMovement playerMovement,
        Animator animator,
        AudioSource audioSource,
        AudioClip jumpSound
    )
    {
        this.playerSpacialDetector = playerSpacialDetector;
        this.playerMovement = playerMovement;
        this.animator = animator;
        this.audioSource = audioSource;
        this.jumpSound = jumpSound;
    }

    public void OnJump()
    {
        if (playerSpacialDetector.IsGrounded(0.1f, 0))
        {
            playerMovement.ApplyJump(jumpStrength);
            animator.SetBool("isJumping", true);
            PlayJumpSound();
        }
    }

    public void PlayJumpSound() 
    {
        if (audioSource != null && jumpSound != null)
        {
            audioSource.PlayOneShot(jumpSound);
        }
    }
}
