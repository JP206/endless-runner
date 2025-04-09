using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;
    private int currentLives;

    private Animator animator;
    private AudioClip hurtSound;
    private AudioClip deathSound;
    private AudioSource audioSource;
    private AudioSource backgroundMusic;

    private Collider2D[] colliders;
    private bool isDead = false;
    private bool hasUsedSaveMe = false;

    [Header("Canvases")]
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameOverImage;
    [SerializeField] private GameObject saveMeCanvas;

    [Header("UI")]
    private List<Image> hearts = new List<Image>();
    [SerializeField] private Transform healthContainer;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    [Header("Invincibility Settings")]
    [SerializeField] private float invincibilityDuration = 3f;

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
        currentLives = maxLives;
        colliders = GetComponentsInChildren<Collider2D>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();

        foreach (Transform child in healthContainer)
        {
            Image heartImage = child.GetComponent<Image>();
            if (heartImage != null) hearts.Add(heartImage);
        }
    }

    public void TakeDamage()
    {
        if (isDead) return;

        if (currentLives - 1 <= 0 && PlayerData.GetItemCount("Invincible") > 0)
        {
            if (PlayerData.UseItem("Invincible"))
            {
                StartCoroutine(ApplyTemporaryInvincibility());
                return;
            }
        }

        currentLives--;
        PlayHurtSound();
        UpdateHealthUI();
        StartCoroutine(HandleDamageEffects());

        if (currentLives <= 0)
            Die();
    }

    private void UpdateHealthUI()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].sprite = (i < currentLives) ? fullHeart : emptyHeart;
        }
    }

    private IEnumerator HandleDamageEffects()
    {
        if (isDead) yield break;

        animator.SetBool("isHitted", true);

        yield return new WaitForSeconds(0.5f);

        animator.SetBool("isHitted", false);
    }

    public void Die()
    {
        if (PlayerData.GetItemCount("Invincible") > 0)
        {
            Debug.Log("PlayerData.GetItemCount: " + PlayerData.GetItemCount("Invincible"));

            if (PlayerData.UseItem("Invincible"))
            {
                Debug.Log("🛡 Invencibilidad activada desde Die(). Se consumió 'Invincible'.");
                StartCoroutine(ApplyTemporaryInvincibility());
                return;
            }
        }

        if (isDead) return;
        isDead = true;

        FindAnyObjectByType<ScoreManager>().GetCoins();

        animator.SetTrigger("isDead");
        animator.SetBool("isHitted", false);

        FreezeScene();
        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator ApplyTemporaryInvincibility()
    {
        Debug.Log("✅ Invencibilidad TEMPORAL activada");
        currentLives = 1;
        UpdateHealthUI();

        // Guardar y quitar el tag
        string originalTag = gameObject.tag;
        gameObject.tag = "Untagged";

        float elapsed = 0f;
        float blinkInterval = 0.1f;

        while (elapsed < invincibilityDuration)
        {
            if (playerSprite != null)
                playerSprite.enabled = !playerSprite.enabled;

            yield return new WaitForSecondsRealtime(blinkInterval);
            elapsed += blinkInterval;
        }

        if (playerSprite != null)
            playerSprite.enabled = true;

        gameObject.tag = originalTag;
        isDead = false;
    }

    public void Revive()
    {
        isDead = false;
    }

    private void FreezeScene()
    {
        Time.timeScale = 0;
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        if (!hasUsedSaveMe && saveMeCanvas != null)
        {
            hasUsedSaveMe = true;
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
        backgroundMusic.gameObject.SetActive(false);
    }
}
