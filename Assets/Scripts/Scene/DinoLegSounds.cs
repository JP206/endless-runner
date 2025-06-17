using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Collider2D))]
public class DinoLegSounds : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip dinoLegSound;

    public void PlayDinoLegSound()
    {
        if (audioSource != null && dinoLegSound != null)
        {
            audioSource.PlayOneShot(dinoLegSound);
            Debug.Log("Ejecute sonido");
        }
    }
}
