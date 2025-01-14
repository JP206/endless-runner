using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    ObstaclePool pool;
    float time = 0;
    Vector3 highPos = new Vector3(9.31f, -2f, 0.02589723f);

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
                if (obstacle.GetComponent<Obstacle>().GetPosition().Equals(ObstaclePosition.high))
                {
                    obstacle.transform.position = highPos;
                }
                else
                {
                    obstacle.transform.position = transform.position;
                }
            }
        }

        time += Time.deltaTime;
    }
}
