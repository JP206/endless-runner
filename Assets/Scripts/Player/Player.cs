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
}
