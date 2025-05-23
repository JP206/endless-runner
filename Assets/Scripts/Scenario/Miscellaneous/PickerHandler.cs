using UnityEngine;

public class PickerHandler : MonoBehaviour
{
    [SerializeField] private AudioClip coinClip;
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
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Dino Leg"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void PlayCoinSound(Vector3 position)
    {
        if (coinClip != null)
        {
            AudioSource.PlayClipAtPoint(coinClip, position);
        }
    }
}
