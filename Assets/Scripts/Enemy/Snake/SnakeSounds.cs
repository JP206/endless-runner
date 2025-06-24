using UnityEngine;

public class SnakeSounds : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private AudioClip rattleClip;
    [SerializeField] private AudioClip damageClip;

    public void PlaySnakeDeathSound()
    {
        if (audioSource != null && deathClip != null)
        {
            audioSource.PlayOneShot(deathClip);
            Debug.Log("▶ Sonido de muerte ejecutado");
        }
    }

    public void PlayRattleSound()
    {
        if (audioSource != null && rattleClip != null)
        {
            audioSource.PlayOneShot(rattleClip);
            Debug.Log("▶ Sonido de cascabel ejecutado");
        }
    }

    public void PlayDamageSound()
    {
        if (audioSource != null && damageClip != null)
        {
            audioSource.PlayOneShot(damageClip);
            Debug.Log("▶ Sonido de daño ejecutado");
        }
    }
}
