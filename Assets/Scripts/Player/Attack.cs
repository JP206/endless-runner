using UnityEngine;

public class Attack : MonoBehaviour
{
    Animator animator;

    [SerializeField] float attackRange = 5f;

    public void InitializeReferences(Animator animator)
    {
        this.animator = animator;
    }

    public void OnPerformAttack()
    {
        animator.SetTrigger("attack");
    }
}
