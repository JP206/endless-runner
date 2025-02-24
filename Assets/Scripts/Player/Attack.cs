using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;
    private ProyectilePool proyectilePool;
    private Player player;

    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float attackCooldown = 0.3f;
    private float nextAttackTime = 0f;

    public void InitializeReferences(
        Animator animator,
        PlayerMovement playerMovement,
        ProyectilePool proyectilePool,
        Player player)
    {
        this.animator = animator;
        this.playerMovement = playerMovement;
        this.player = player;
        this.proyectilePool = proyectilePool;
    }

    public void PerformAttack()
    {
        if (Time.time < nextAttackTime) return;

        if (playerMovement.isJumping)
        {
            OnPerformAirAttack();
        }
        else
        {
            OnPerformAttack();
        }

        nextAttackTime = Time.time + attackCooldown;
    }

    private void OnPerformAttack()
    {
        animator.SetTrigger("attack");
        ShootProyectile();
    }

    private void OnPerformAirAttack()
    {
        StartCoroutine(AirAttackCoroutine());
    }

    private IEnumerator AirAttackCoroutine()
    {
        animator.SetBool("airAttack", true);
        ShootProyectile();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetBool("airAttack", false);
    }

    public void ShootProyectile()
    {
        GameObject proyectile = proyectilePool.GetPooledObject();
        if (proyectile)
        {
            proyectile.transform.position = projectileSpawnPoint.position;
            proyectile.transform.rotation = projectileSpawnPoint.rotation;
        }
    }
}
