using UnityEngine;

public class ShakeOnPlayerHit : MonoBehaviour
{
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private float intensity = 0.05f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && CameraShake.Instance != null)
        {
            Debug.Log("Shake triggered via Trigger!");
            CameraShake.Instance.Shake(duration, intensity);
        }
    }
}
