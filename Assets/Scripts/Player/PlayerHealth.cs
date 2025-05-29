using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private Image healthFillImage;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float damageAmount = 25f;
    [SerializeField] private float recoverAmount = 20f;
    private float currentHealth;

    [Header("Invincibility Settings")]
    [SerializeField] private float invincibilityDuration = 2f;
    private bool isInvulnerable = false;
    private bool isDead = false;

    [Header("Canvases")]
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameOverImage;
    [SerializeField] private GameObject saveMeCanvas;
    private bool hasUsedSaveMe = false;

    [Header("Audio")]
    private AudioSource audioSource;
    private AudioSource backgroundMusic;
    private AudioClip hurtSound;
    private AudioClip deathSound;

    private Animator animator;
    private SpriteRenderer playerSprite;

    public void InitializeReferences(
        Animator animator,
        AudioClip hurtSound,
        AudioClip deathSound,
        AudioSource audioSource,
        AudioSource backgroundMusic
    )
    {
        this.animator = animator;
        this.hurtSound = hurtSound;
        this.deathSound = deathSound;
        this.audioSource = audioSource;
        this.backgroundMusic = backgroundMusic;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        if (healthFillImage != null)
            healthFillImage.fillAmount = 1f;

        playerSprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (!isDead && !isInvulnerable && currentHealth > 0f && Time.timeScale > 0)
        {
            currentHealth -= 5f * Time.deltaTime;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

            if (healthFillImage != null)
                healthFillImage.fillAmount = currentHealth / maxHealth;

            if (currentHealth <= 0 && !isDead)
            {
                HandleZeroHealth();
            }

        }
    }
    private void HandleZeroHealth()
    {
        currentHealth = 0;
        if (healthFillImage != null)
            healthFillImage.fillAmount = 0;

        if (!isDead)
        {
            Die();
        }
    }


    public void TakeDamage()
    {
        if (isDead || isInvulnerable) return;

        float newHealth = currentHealth - damageAmount;

        currentHealth = Mathf.Clamp(newHealth, 0f, maxHealth);

        if (healthFillImage != null)
            healthFillImage.fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            PlayHurtSound();
            Die();
            return;
        }

        PlayHurtSound();
        StartCoroutine(HandleDamageEffects());
        StartCoroutine(HandleInvulnerability());
    }


    private IEnumerator HandleDamageEffects()
    {
        animator.SetBool("isHitted", true);
        yield return new WaitForSeconds(0.5f);
        if (!isDead) animator.SetBool("isHitted", false);
    }

    private IEnumerator HandleInvulnerability()
    {
        isInvulnerable = true;
        SetCollisionWithEnemies(false);
        StartCoroutine(FlashWhileInvulnerable());

        yield return new WaitForSeconds(invincibilityDuration);

        isInvulnerable = false;
        SetCollisionWithEnemies(true);
    }

    private IEnumerator FlashWhileInvulnerable()
    {
        float flashInterval = 0.10f;

        while (isInvulnerable)
        {
            if (playerSprite != null) playerSprite.enabled = false;
            yield return new WaitForSeconds(flashInterval);

            if (playerSprite != null) playerSprite.enabled = true;
            yield return new WaitForSeconds(flashInterval);
        }

        if (playerSprite != null) playerSprite.enabled = true;
    }

    private void SetCollisionWithEnemies(bool shouldCollide)
    {
        GroundedEnemy[] enemies = Object.FindObjectsByType<GroundedEnemy>(FindObjectsSortMode.None);
        foreach (GroundedEnemy enemy in enemies)
        {
            enemy.IgnoreCollisionWithPlayer(!shouldCollide);
        }
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        PokiScript.Instance?.CallGameplayStop();

        animator.SetTrigger("isDead");
        animator.SetBool("isHitted", false);

        DisablePlayerColliders();

        FindFirstObjectByType<ScoreManager>()?.GetCoins();
        StartCoroutine(DeathSequence());
    }


    private IEnumerator DeathSequence()
    {
        FreezeScene();
        yield return StartCoroutine(DeathMotion());
        StartCoroutine(WaitForDeathAnimation());
    }


    public void SetExternalInvulnerability(bool value)
    {
        isInvulnerable = value;
        SetCollisionWithEnemies(!value);
    }
    public bool IsDead()
    {
        return isDead;
    }

    public void Revive()
    {
        isDead = false;
        hasUsedSaveMe = true;

        currentHealth = maxHealth;

        if (healthFillImage != null)
            healthFillImage.fillAmount = 1f;
    }

    private void FreezeScene()
    {
        Time.timeScale = 0;
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void DisablePlayerColliders()
    {
        foreach (var col in GetComponentsInChildren<Collider2D>())
            col.enabled = false;
    }


    private IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("Death")
        );

        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f
        );

        if (!hasUsedSaveMe && saveMeCanvas != null)
        {
            ShowSaveMeCanvas();
        }
        else
        {
            ShowGameOverCanvas();
        }
    }


    private void ShowSaveMeCanvas()
    {
        gameOverCanvas.SetActive(true);
        saveMeCanvas.SetActive(true);

        PlayDeathSound();
        StopBackgroundMusic();

        PokiScript.Instance?.CallGameplayStop();

        StartCoroutine(FadeInPanel());
        StartCoroutine(AnimateGameOverImage());
    }


    private void ShowGameOverCanvas()
    {
        gameOverCanvas.SetActive(true);
        gameOverImage.SetActive(true);
        PlayDeathSound();
        StopBackgroundMusic();
        StartCoroutine(FadeInPanel());
        StartCoroutine(AnimateGameOverImage());
    }

    private IEnumerator FadeInPanel()
    {
        CanvasGroup canvasGroup = gameOverPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameOverPanel.AddComponent<CanvasGroup>();

        Image panelImage = gameOverPanel.GetComponent<Image>();
        if (panelImage == null) panelImage = gameOverPanel.AddComponent<Image>();

        panelImage.color = new Color(0, 0, 0, 0);

        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / duration);
            panelImage.color = new Color(0, 0, 0, Mathf.Lerp(0, 0.8f, elapsed / duration));
            yield return null;
        }

        canvasGroup.alpha = 1;
        panelImage.color = new Color(0, 0, 0, 0.8f);
    }

    private IEnumerator DeathMotion()
    {
        float duration = 1f;
        float peakHeight = 2f;

        Vector3 start = transform.position;
        Vector3 peak = start + Vector3.up * peakHeight;
        Vector3 end = start + Vector3.down * 3f;

        float elapsed = 0f;

        // Subir
        while (elapsed < duration / 2f)
        {
            elapsed += Time.unscaledDeltaTime;
            transform.position = Vector3.Lerp(start, peak, elapsed / (duration / 2f));
            yield return null;
        }

        elapsed = 0f;

        // Bajar
        while (elapsed < duration / 2f)
        {
            elapsed += Time.unscaledDeltaTime;
            transform.position = Vector3.Lerp(peak, end, elapsed / (duration / 2f));
            yield return null;
        }

        transform.position = end;
    }


    private IEnumerator AnimateGameOverImage()
    {
        RectTransform imageTransform = gameOverImage.GetComponent<RectTransform>();
        Vector3 startPosition = imageTransform.anchoredPosition + new Vector2(0, -200);
        Vector3 endPosition = imageTransform.anchoredPosition;

        float duration = 1f;
        float elapsed = 0f;

        imageTransform.anchoredPosition = startPosition;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            imageTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, elapsed / duration);
            yield return null;
        }

        imageTransform.anchoredPosition = endPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dino Leg"))
        {
            currentHealth += recoverAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

            if (healthFillImage != null)
                healthFillImage.fillAmount = currentHealth / maxHealth;

            Destroy(collision.gameObject);
        }
    }

    public void PlayDeathSound()
    {
        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);
    }

    public void PlayHurtSound()
    {
        if (audioSource != null && hurtSound != null)
            audioSource.PlayOneShot(hurtSound);
    }

    public void StopBackgroundMusic()
    {
        if (backgroundMusic != null)
            backgroundMusic.gameObject.SetActive(false);
    }
}
