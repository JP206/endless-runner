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

    [Header("Sounds Click")]
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private AudioSource pauseSound;

    [Header("Buttons")]
    [SerializeField] private Transform retryButtonTransform;
    [SerializeField] private Transform mainMenuButtonTransform;
    [SerializeField] private Transform resumeButtonTransform;
    [SerializeField] private Transform exitButtonTransform;

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
            GetPauseSound();
            PauseGame();
        }
    }

    public void LoadScene(string sceneName, bool resetTime = true)
    {
        if (resetTime)
            Time.timeScale = 1;

        SceneManager.LoadScene(sceneName);
    }

    public void ReloadScene()
    {
        StartCoroutine(AnimateButtonPress(retryButtonTransform));
        GetClickSound();
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        StartCoroutine(AnimateButtonPress(mainMenuButtonTransform));
        GetClickSound();
        LoadScene("MainMenu");
    }

    public void PauseGame()
    {
        ToggleGameState(true);
    }

    public void ResumeGame()
    {
        StartCoroutine(AnimateButtonPress(resumeButtonTransform));
        GetClickSound();
        ToggleGameState(false);
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
        LoadScene("Seba");
    }

    public void ExitGame()
    {
        StartCoroutine(AnimateButtonPress(exitButtonTransform));
        GetClickSound();
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

    private IEnumerator AnimateButtonPress(Transform target)
    {
        if (target == null) yield break;

        Vector3 originalScale = target.localScale;
        Vector3 pressedScale = originalScale * 0.95f;
        float duration = 0.05f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            target.localScale = Vector3.Lerp(originalScale, pressedScale, elapsed / duration);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            target.localScale = Vector3.Lerp(pressedScale, originalScale, elapsed / duration);
            yield return null;
        }

        target.localScale = originalScale;
    }
}