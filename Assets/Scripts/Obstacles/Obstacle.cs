using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float speed = 5f; // Velocidad de movimiento
    [SerializeField] float posY; // Altura fija del obstáculo
    [SerializeField] float xThreshold = -10f; // Límite para desactivar el obstáculo

    void Update()
    {
        // Mueve el obstáculo hacia la izquierda
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    void OnBecameInvisible()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }

    public float GetPosY()
    {
        return posY;
    }
}
