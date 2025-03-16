using UnityEngine;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] int spawnRate;
    [SerializeField] float spawnOffset; // Distancia fuera de la cámara donde se generan los obstaculos

    ObstaclePool pool;
    float time = 0;
    float lastObstaclePositionX = 0;
    float lastObstacleWidth = 0;

    void Start()
    {
        pool = GetComponent<ObstaclePool>();
        lastObstaclePositionX = transform.position.x;
    }

    void Update()
    {
        time += Time.deltaTime;

        // Verifico si hay obstáculos visibles antes de generar otro
        if (time >= spawnRate && !IsAnyObstacleVisible())
        {
            time = 0;
            SpawnObstacle();
        }
    }
    void SpawnObstacle()
    {
        GameObject obstacle = pool.GetRandomObstacle();
        if (obstacle)
        {
            float obstacleWidth = GetObstacleWidth(obstacle);

            float minSeparation = 0.1f;
            float maxSeparation = 1.5f;

            // Calculo una separacion dentro del rango de seperacion
            float separation = Random.Range(minSeparation, maxSeparation);

            // posicion en eje X --> Justo después del ultimo obstaculo + separacion
            float newX = lastObstaclePositionX + (lastObstacleWidth / 2) + (obstacleWidth / 2) + separation;

            // Ajusto la posicion para evitar superposiciones y agregar separacion si es necesario
            newX = GetValidSpawnPosition(newX, obstacleWidth);

            // Asignar la nueva posición al obstáculo
            obstacle.transform.position = new Vector3(newX, obstacle.GetComponent<Obstacle>().GetPosY(), 0);

            // Guardar la nueva posición y ancho para el siguiente obstáculo
            lastObstaclePositionX = newX + (obstacleWidth / 2);
            lastObstacleWidth = obstacleWidth;
        }
    }

    float GetValidSpawnPosition(float startX, float obstacleWidth)
    {
        float checkX = startX;
        bool positionIsFree = false;
        int maxAttempts = 15; // Evita bucles infinitos
        float extraSeparation = 2f; // Si hay solapamiento fuera de camara, aumento la separacion

        while (!positionIsFree && maxAttempts > 0)
        {
            positionIsFree = true;
            foreach (var existingObstacle in pool.GetAllObstacles())
            {
                if (existingObstacle.activeInHierarchy)
                {
                    Collider2D col = existingObstacle.GetComponent<Collider2D>();
                    if (col != null)
                    {
                        float leftEdge = col.bounds.min.x;
                        float rightEdge = col.bounds.max.x;

                        // Si el nuevo obstaculo colisiona con otro, lo alejo
                        if ((checkX >= leftEdge && checkX <= rightEdge) || (checkX + obstacleWidth >= leftEdge && checkX + obstacleWidth <= rightEdge))
                        {
                            positionIsFree = false;
                            checkX += extraSeparation;
                            break;
                        }
                    }
                }
            }
            maxAttempts--;
        }
        return checkX;
    }


    bool IsAnyObstacleVisible()
    {
        foreach (var obstacle in pool.GetAllObstacles())
        {
            if (obstacle.activeInHierarchy && IsVisibleToCamera(obstacle))
            {
                return true;
            }
        }
        return false;
    }

    bool IsVisibleToCamera(GameObject obstacle)
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(obstacle.transform.position);
        return screenPoint.x > 0 && screenPoint.x < 1;
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
