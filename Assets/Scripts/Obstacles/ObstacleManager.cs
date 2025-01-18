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
            GameObject obstacle = pool.GetRandomObstacle();
            if (obstacle)
            {
                obstacle.transform.position = new Vector3(transform.position.x, obstacle.GetComponent<Obstacle>().GetPosY(), 0);
            }
        }

        time += Time.deltaTime;
    }
}
