using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;

    public float airAttackDuration = 1f;

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
        StartCoroutine(AirAttackCoroutine());
    }

    private IEnumerator AirAttackCoroutine()
    {
        animator.SetBool("airAttack", true);

        yield return new WaitForSeconds(airAttackDuration);

        animator.SetBool("airAttack", false);
    }
}
