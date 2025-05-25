using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    private int score = 0;

    void Start()
    {
        ResetScore();
    }
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    public void GetCoins()
    {
        int coins = score / 100;
        PlayerData.AddCoins(coins);

        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }

        PlayerPrefs.SetInt("LastScore", score);
        PlayerPrefs.Save();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }
}
