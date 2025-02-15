using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

    public void InitializeReferences(PlayerMovement playerMovement, PlayerHealth playerHealth)
    {
        this.playerMovement = playerMovement;
        this.playerHealth = playerHealth;
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
}
