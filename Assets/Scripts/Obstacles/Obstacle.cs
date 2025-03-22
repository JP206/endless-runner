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

    public float GetPosY()
    {
        return posY;
    }
}
