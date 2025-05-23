using UnityEngine;

public class PickerHandler : MonoBehaviour
{
    [SerializeField] private AudioClip coinClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
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
