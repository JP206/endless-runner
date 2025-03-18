using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float posY;
    [SerializeField] float xThreshold = -10f;

    void Update()
    {
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
