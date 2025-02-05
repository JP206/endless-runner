using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;
    private int currentLives;

    private Animator animator;
    private Collider2D[] colliders;
    private bool isDead = false; 
    private float moveSpeed = 6f;
    private float riseTime = 0.4f;

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
        if (isDead) return;

        currentLives--;

        StartCoroutine(HandleDamageEffects());

        if (currentLives <= 0)
        {
            Die();
        }
    }

    private IEnumerator HandleDamageEffects()
    {
        if (isDead) yield break;

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
        if (isDead) return;
        isDead = true;

        animator.SetTrigger("isDead");
        animator.SetBool("isHitted", false);

        SetCollidersActive(false);

        FreezeScene();

        StartCoroutine(HandleDeathFall());
    }

    private void FreezeScene()
    {
        //Time.timeScale = 0;
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void DisableFloorColliders()
    {
        foreach (GameObject floor in GameObject.FindGameObjectsWithTag("Floor"))
        {
            Collider2D floorCollider = floor.GetComponent<Collider2D>();
            if (floorCollider != null)
            {
                floorCollider.enabled = false;
            }
        }
    }

    private IEnumerator HandleDeathFall()
    {
        float elapsed = 0f;
        while (elapsed < riseTime)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.unscaledDeltaTime);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        DisableFloorColliders();

        while (true)
        {
            transform.Translate(Vector3.down * (moveSpeed * 1) * Time.unscaledDeltaTime);
            yield return null;
        }
    }

    public int GetLives()
    {
        return currentLives;
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        currentLives = Mathf.Min(currentLives + amount, maxLives);
    }
}
