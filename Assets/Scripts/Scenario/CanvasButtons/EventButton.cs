using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class EventButton : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public GameObject pauseImage;
    public Image fadeOverlay;
    public MonoBehaviour inputController;

    [SerializeField] PlayerHealth playerHealth;

    [Header("Sounds Click")]
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private AudioSource pauseSound;
    [SerializeField] private AudioSource backgroundMusic;

    [Header("Buttons")]
    [SerializeField] private Image retryButtonImage;
    [SerializeField] private Image mainMenuButtonImage;
    [SerializeField] private Image resumeButtonImage;
    [SerializeField] private Image exitButtonImage;

    private bool isPaused = false;

    void Start()
    {
        pauseMenuCanvas.SetActive(false);
        if (pauseImage != null) pauseImage.SetActive(false);

        if (inputController != null)
            inputController.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            PauseGame();
        }
    }

    public void SaveMeButton()
    {
        GetClickSound();
        
        FindFirstObjectByType<RevivePlayer>()?.revivePlayer();
        ResumeBackgroundMusic();

        //PokiScript.Instance?.CallRewardedBreak((bool withReward) =>
        //{
        //    if (withReward)
        //    {
        //        FindFirstObjectByType<RevivePlayer>()?.revivePlayer();
        //        PokiScript.Instance?.CallGameplayStart();
        //        ResumeBackgroundMusic();
        //    }
        //    else
        //    {
        //        Debug.Log("❌ Anuncio no completado. No se revive.");
        //    }
        //});
    }

    public void LoadScene(string sceneName, bool resetTime = true)
    {
        if (resetTime)
            Time.timeScale = 1;

        SceneManager.LoadScene(sceneName);
    }

    public void ReloadScene()
    {
        GetClickSound();
        StartCoroutine(AnimateButtonPress(retryButtonImage));
        LoadScene(SceneManager.GetActiveScene().name);

    }

    public void LoadMainMenu()
    {
        GetClickSound();
        StartCoroutine(AnimateButtonPress(mainMenuButtonImage));
        LoadScene("MainMenu");
    }

    public void PauseGame()
    {
        GetPauseSound();
        ToggleGameState(true);

        //PokiScript.Instance?.CallGameplayStop();
    }


    public void ResumeGame()
    {
        GetClickSound();
        StartCoroutine(AnimateButtonPress(resumeButtonImage));
        ToggleGameState(false);

        //PokiScript.Instance?.CallGameplayStart();
    }


    public void GameOver()
    {
        ToggleGameState(true);
    }

    private void ToggleGameState(bool pause)
    {
        isPaused = pause;
        Time.timeScale = pause ? 0 : 1;

        if (pauseMenuCanvas != null)
            pauseMenuCanvas.SetActive(pause);

        if (pauseImage != null)
            pauseImage.SetActive(pause);

        if (fadeOverlay != null)
            fadeOverlay.color = new Color(0, 0, 0, pause ? 0.5f : 0);

        if (inputController != null)
            inputController.enabled = !pause;
    }

    public void PlayGame()
    {
        GetClickSound();
        //PokiScript.Instance?.CallGameplayStart();
        LoadScene("Game");
    }

    public void ExitGame()
    {
        GetClickSound();
        StartCoroutine(AnimateButtonPress(exitButtonImage));
        Application.Quit();
    }

    private void GetClickSound()
    {
        if (clickSound != null && clickSound.clip != null)
            clickSound.PlayOneShot(clickSound.clip);
    }

    private void GetPauseSound()
    {
        if (pauseSound != null && pauseSound.clip != null)
            pauseSound.PlayOneShot(pauseSound.clip);
    }
    public void ResumeBackgroundMusic()
    {
        if (backgroundMusic != null && backgroundMusic.clip != null)
            backgroundMusic.gameObject.SetActive(true);
            backgroundMusic.PlayOneShot(backgroundMusic.clip);
    }

    private IEnumerator AnimateButtonPress(Image buttonImage)
    {
        if (buttonImage == null) yield break;

        RectTransform rect = buttonImage.rectTransform;
        Vector3 originalScale = rect.localScale;
        Vector3 pressedScale = originalScale * 0.95f;
        float duration = 0.05f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            rect.localScale = Vector3.Lerp(originalScale, pressedScale, elapsed / duration);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            rect.localScale = Vector3.Lerp(pressedScale, originalScale, elapsed / duration);
            yield return null;
        }

        rect.localScale = originalScale;
    }
}
