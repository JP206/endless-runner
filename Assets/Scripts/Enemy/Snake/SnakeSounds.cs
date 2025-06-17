using UnityEngine;
using UnityEngine.Audio;

public class SnakeSounds : MonoBehaviour
{
    AudioSource audioSource;
    AudioClip snakeDeathSound;

    public void PlaySnakeDeathSound()
    {
        if (audioSource != null && snakeDeathSound != null)
        {
            audioSource.PlayOneShot(snakeDeathSound);
        }
    }
}
