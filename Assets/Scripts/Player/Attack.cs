using UnityEngine;

public class Attack : MonoBehaviour
{
    Animator animator;
    PlayerMovement playerMovement;

    public void InitializeReferences(Animator animator, PlayerMovement playerMovement)
    {
        this.animator = animator;
        this.playerMovement = playerMovement;
    }
    public void PerformAttack()
    {

        if (playerMovement.isJumping)
        {
            OnPerformAirAttack();
        }
        else
        {
            OnPerformAttack();
        }
    }

    private void OnPerformAttack()
    {
        animator.SetTrigger("attack");
    }

    private void OnPerformAirAttack()
    {
        animator.SetTrigger("airAttack");
    }
}
