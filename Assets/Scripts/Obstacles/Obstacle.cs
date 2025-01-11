using UnityEngine;

public class Obstacle : MonoBehaviour
{
    void Update()
    {
        int layerIndex = gameObject.layer;

        if (transform.position.x < -10)
        {
            Destroy(gameObject);
        }

        transform.position -= new Vector3(5 * Time.deltaTime, 0, 0);
    }
}
