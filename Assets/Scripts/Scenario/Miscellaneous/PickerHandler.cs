using UnityEngine;

public class PickerHandler : MonoBehaviour
{
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip dinoLegSound;
    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = Object.FindFirstObjectByType<ScoreManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            PlayerData.AddCoins(1);
            scoreManager?.AddScore(1);
            PlayCoinSound(collision.transform.position);
            collision.gameObject.SetActive(false); 
        }

        else if (collision.CompareTag("Dino Leg"))
        {
            PlayDinoLegSound(collision.transform.position);
            collision.gameObject.SetActive(false);
        }
    }

    private void PlayCoinSound(Vector3 position)
    {
        if (coinClip != null)
        {
            AudioSource.PlayClipAtPoint(coinClip, position);
        }
    }

    private void PlayDinoLegSound(Vector3 position)
    {
        if (dinoLegSound != null)
        {
            AudioSource.PlayClipAtPoint(dinoLegSound, position);
        }
    }
}
