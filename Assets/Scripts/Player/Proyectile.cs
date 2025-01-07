using UnityEngine;

public class Proyectile : MonoBehaviour
{
    void Update()
    {
        if (transform.position.x > 10)
        {
            Destroy(gameObject);
        }

        transform.position += new Vector3(5 * Time.deltaTime, 0, 0);
    }
}
