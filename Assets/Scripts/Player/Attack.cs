using System.Collections;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;
    private ProyectilePool proyectilePool;
    AudioSource audioSource;
    AudioClip throwAxe;

    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private float attackCooldown = 0.3f;
    [SerializeField] private float projectileSpeed = 10f;

    private float nextAttackTime = 0f;

    public void InitializeReferences(
        Animator animator,
        PlayerMovement playerMovement,
        ProyectilePool proyectilePool,
        AudioSource audioSource,
        AudioClip throwAxe
        )
    {
        this.animator = animator;
        this.playerMovement = playerMovement;
        this.proyectilePool = proyectilePool;
        this.audioSource = audioSource;
        this.throwAxe = throwAxe;
    }

    public void PerformAttack()
    {
        if (Time.time < nextAttackTime) return;

        if (playerMovement.isJumping) OnPerformAirAttack();
        else OnPerformAttack();

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
            PlayThrowSound();
            proyectile.transform.position = projectileSpawnPoint.position;
            proyectile.transform.rotation = projectileSpawnPoint.rotation;

            Rigidbody2D rb = proyectile.GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.linearVelocity = new Vector2(projectileSpeed, rb.linearVelocity.y);
            }
        }
    }

    public void PlayThrowSound()
    {
        if (audioSource != null && throwAxe != null)
        {
            audioSource.PlayOneShot(throwAxe);
        }
    }
}
