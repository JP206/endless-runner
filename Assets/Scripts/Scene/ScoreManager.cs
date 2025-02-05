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
        scoreText.text = "Score: " + score;
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time >= scoreRate)
        {
            time = 0;
            score++;
            scoreText.text = "Score: " + score;
        }
    }
}
