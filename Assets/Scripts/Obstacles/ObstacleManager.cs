using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    ObstaclePool pool;
    float time = 0;

    void Start()
    {
        pool = GetComponent<ObstaclePool>();
    }

    void Update()
    {
        if (time >= 3)
        {
            time = 0;
            GameObject obstacle = pool.GetPooledObject();
            if (obstacle)
            {
                obstacle.transform.position = transform.position;
            }
        }

        time += Time.deltaTime;
    }
}
