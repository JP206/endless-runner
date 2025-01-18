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
        PerformAttack();
        animator.SetTrigger("attack");
    }

    public void PerformAttack()
    {
        Enemy _enemy = playerSpacialDetector.DetectEnemy(transform.localScale.x, attackRange);
        if (_enemy != null) {
            //_enemy.DestroyEnemy();
            Debug.Log("GOLPEE ENEMIGO"); 
        }
    }
}
