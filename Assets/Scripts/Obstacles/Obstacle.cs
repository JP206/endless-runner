using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] ObstaclePosition position;

    void Update()
    {
        int layerIndex = gameObject.layer;

        if (transform.position.x < -10)
        {
            gameObject.SetActive(false);
        }

        transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
    }

    public ObstaclePosition GetPosition()
    {
        return position;
    }
}

public enum ObstaclePosition
{
    low,
    medium,
    high
}