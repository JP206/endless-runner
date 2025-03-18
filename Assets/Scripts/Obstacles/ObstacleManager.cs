using UnityEngine;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private int spawnRate;
    [SerializeField] private float spawnOffset;

    private ObstaclePool pool;
    private float lastObstaclePositionX = 0;
    private float lastObstacleWidth = 0;
    private GameObject lastSpawnedObstacle;
    float camMaxX => Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0f, 0f)).x;

    void Start()
    {
        pool = Object.FindFirstObjectByType<ObstaclePool>();

        if (pool == null)
        {
            Debug.LogError("ObstaclePool no encontrado. Asegúrate de que hay un ObstaclePool en la escena.");
        }
        lastObstaclePositionX = transform.position.x;
    }

    void Update()
    {
        if (lastSpawnedObstacle == null || IsObstacleOutOfView(lastSpawnedObstacle))
        {
            SpawnObstacle();
        }
    }

    void SpawnObstacle()
    {
        if (pool == null) return;
        GameObject obstacle = pool.GetRandomObstacle();
        if (obstacle == null) return;

        float obstacleWidth = GetObstacleWidth(obstacle);
        float minSeparation = 0.1f;
        float maxSeparation = 0.5f;
        float separation = Random.Range(minSeparation, maxSeparation);

        float newX = lastObstaclePositionX + lastObstacleWidth + separation - GetLeftEdge(obstacle);

        obstacle.transform.position = new Vector3(newX, obstacle.transform.position.y, 0);

        lastSpawnedObstacle = obstacle;
        lastObstaclePositionX = newX;
        lastObstacleWidth = obstacleWidth;
    }

    bool IsObstacleOutOfView(GameObject obstacle)
    {
        float cameraLeftEdge = camMaxX;
        return obstacle.transform.position.x + lastObstacleWidth < cameraLeftEdge;
    }

    float GetLeftEdge(GameObject obj)
    {
        Collider2D col = obj.GetComponent<Collider2D>();
        if (col != null)
        {
            return col.bounds.min.x;
        }
        return obj.transform.position.x;
    }

    float GetObstacleWidth(GameObject obstacle)
    {
        Collider2D col = obstacle.GetComponent<Collider2D>();
        if (col != null)
        {
            return col.bounds.size.x;
        }

        SpriteRenderer sprite = obstacle.GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            return sprite.bounds.size.x;
        }

        return 5f;
    }
}
 