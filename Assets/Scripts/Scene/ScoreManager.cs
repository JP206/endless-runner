using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] float scoreRate;
    [SerializeField] TextMeshProUGUI scoreText;

    int score = 0;
    float time = 0;

    void Start()
    {
        scoreText.text = score.ToString();
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= scoreRate)
        {
            time = 0;
            score++;
            UpdateScoreUI();
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        scoreText.text = score.ToString();
    }

    public void GetCoins()
    {
        // se llama cuando se termina el juego
        int coins = (int)(score / 100);
        PlayerData.AddCoins(coins);
        if (score > PlayerPrefs.GetInt("score"))
        {
            PlayerPrefs.SetInt("score", score);
        }
    }
}
