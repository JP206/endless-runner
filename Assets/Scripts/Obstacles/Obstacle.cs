using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float speed = 5f; // Velocidad de movimiento
    [SerializeField] float posY; // Altura fija del obst�culo
    [SerializeField] float xThreshold = -10f; // L�mite para desactivar el obst�culo

    void Update()
    {
        // Mueve el obst�culo hacia la izquierda
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
