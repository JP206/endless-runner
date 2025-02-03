using UnityEngine;
using System.Collections; // Necesario para usar corutinas

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;
    private int currentLives;

    private Animator animator;
    private Collider2D[] colliders; 

    public void InitializeReferences(Animator animator)
    {
        this.animator = animator;
    }

    private void Start()
    {
        currentLives = maxLives;
        colliders = GetComponentsInChildren<Collider2D>();
    }

    public void TakeDamage()
    {
        currentLives--;

        Debug.Log($"Vidas restantes: {currentLives}");

        StartCoroutine(HandleDamageEffects());

        if (currentLives <= 0)
        {
            Die();
        }
    }

    private IEnumerator HandleDamageEffects()
    {
        animator.SetBool("isHitted", true);

        SetCollidersActive(false);

        yield return new WaitForSeconds(0.5f);

        SetCollidersActive(true);

        animator.SetBool("isHitted", false);
    }

    private void SetCollidersActive(bool isActive)
    {
        foreach (var collider in colliders)
        {
            collider.enabled = isActive;
        }
    }

    private void Die()
    {
        Debug.Log("Jugador muerto");
    }

    public int GetLives()
    {
        return currentLives;
    }

    public void Heal(int amount)
    {
        currentLives = Mathf.Min(currentLives + amount, maxLives);
    }
}
