using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerHealth playerHealth;
    AudioClip stepSound;
    AudioSource audioSource;

    public void InitializeReferences(
        PlayerMovement playerMovement, 
        PlayerHealth playerHealth, 
        AudioClip stepSound,
        AudioSource audioSource
    )
    {
        this.playerMovement = playerMovement;
        this.playerHealth = playerHealth;
        this.stepSound = stepSound;
        this.audioSource = audioSource;
    }

    private void FixedUpdate()
    {
        playerMovement.MovementY();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBack"))
        {
            Enemy enemy = collision.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.DestroyEnemy();
                playerMovement.ApplyJump(5f);
            }
        }
    }

    public void PlayStepSounds()
    {
        if (stepSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(stepSound);
        }
    }
}
