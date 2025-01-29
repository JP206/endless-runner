using UnityEngine;

public class Attack : MonoBehaviour
{
    Animator animator;
    PlayerSpacialDetector playerSpacialDetector;

    [SerializeField] float attackRange = 5f;

    public void InitializeReferences(Animator animator, PlayerSpacialDetector playerSpacialDetector)
    {
        this.animator = animator;
        this.playerSpacialDetector = playerSpacialDetector;
    }

    public void OnPerformAttack()
    {
        animator.SetTrigger("attack");
    }
}
