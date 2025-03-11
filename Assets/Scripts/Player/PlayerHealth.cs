using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;
    private int currentLives;

    private Animator animator;
    private Collider2D[] colliders;
    private bool isDead = false;

    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameOverImage;

    [SerializeField] private Transform healthContainer; // Contenedor de corazones
    private List<Image> hearts = new List<Image>();
    [SerializeField] private Sprite fullHeart; // Imagen del corazón rojo
    [SerializeField] private Sprite emptyHeart; // Imagen del corazón gris

    public void InitializeReferences(Animator animator)
    {
        this.animator = animator;
    }

    private void Start()
    {
        currentLives = maxLives;
        colliders = GetComponentsInChildren<Collider2D>();

        // Obtener las imágenes de los corazones desde el contenedor
        foreach (Transform child in healthContainer)
        {
            Image heartImage = child.GetComponent<Image>();
            if (heartImage != null)
            {
                hearts.Add(heartImage);
            }
        }
    }

    public void TakeDamage()
    {
        if (isDead) return;

        currentLives--;

        UpdateHealthUI(); // Actualizar la UI de vidas

        StartCoroutine(HandleDamageEffects());

        if (currentLives <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < currentLives)
            {
                hearts[i].sprite = fullHeart; // Mantiene los corazones rojos
            }
            else
            {
                hearts[i].sprite = emptyHeart; // Cambia a corazón gris
            }
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

        StartCoroutine(WaitForDeathAnimation());
    }

    private void FreezeScene()
    {
        Time.timeScale = 0;
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        ShowGameOverCanvas();
    }

    private void ShowGameOverCanvas()
    {
        gameOverCanvas.SetActive(true);
        gameOverImage.SetActive(true);

        StartCoroutine(FadeInPanel());
        StartCoroutine(AnimateGameOverImage());
    }

    private IEnumerator FadeInPanel()
    {
        CanvasGroup canvasGroup = gameOverPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameOverPanel.AddComponent<CanvasGroup>();

        UnityEngine.UI.Image panelImage = gameOverPanel.GetComponent<UnityEngine.UI.Image>();
        if (panelImage == null) panelImage = gameOverPanel.AddComponent<UnityEngine.UI.Image>();

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
}
