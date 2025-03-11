using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventButton : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public GameObject pauseImage;
    public Image fadeOverlay;
    public MonoBehaviour inputController;

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

    public void LoadScene(string sceneName, bool resetTime = true)
    {
        if (resetTime)
            Time.timeScale = 1;

        SceneManager.LoadScene(sceneName);
    }

    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        LoadScene("MainMenu");
    }

    public void PauseGame()
    {
        ToggleGameState(true);
    }

    public void ResumeGame()
    {
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
        LoadScene("Game");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
