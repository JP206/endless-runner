using UnityEngine;
using System.Collections;

public class SnakeDamage : MonoBehaviour
{
    private bool hasDealtDamage = false;
    private bool isDead = false;

    [SerializeField] private float damageCooldown = 1.5f;
    [SerializeField] private bool canDamage = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead || !canDamage) return;

        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null && !player.IsDead())
            {
                player.TakeDamage();
                StartCoroutine(DamageCooldown());
            }
        }
    }

    private IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSecondsRealtime(damageCooldown);
        canDamage = true;
    }

    public void SetDead()
    {
        isDead = true;
        canDamage = false;
    }

    private void OnEnable()
    {
        canDamage = true;
        isDead = false;
    }

    public void ResetState()
    {
        StopAllCoroutines();
        hasDealtDamage = false;
        isDead = false;
        canDamage = true;
        gameObject.SetActive(true);
    }
}
