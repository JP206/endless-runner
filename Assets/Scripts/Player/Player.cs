using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerMovement playerMovement;
    public void InitializeReferences(PlayerMovement playerMovement)
    {
        this.playerMovement = playerMovement;
    }

    void FixedUpdate()
    {
        //playerMovement.MovementY();
    }
}
