using UnityEngine;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] int spawnRate;
    [SerializeField] float spawnOffset; // Distancia fuera de la c�mara donde se generan los obst�culos

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

        // Verifico si hay obst�culos visibles antes de generar otro
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

            // Calcular la nueva posici�n en X tomando la posici�n final del �ltimo obst�culo
            float newX = lastObstaclePositionX + (lastObstacleWidth / 2) + (obstacleWidth / 2) + 2f;

            // Si detectamos que la posici�n est� ocupada, desplazamos hasta encontrar un lugar vac�o
            newX = GetValidSpawnPosition(newX, obstacleWidth);

            // Asignar la nueva posici�n al obst�culo
            obstacle.transform.position = new Vector3(newX, obstacle.GetComponent<Obstacle>().GetPosY(), 0);

            // Guardar la nueva posici�n y ancho para el siguiente obst�culo
            lastObstaclePositionX = newX + (obstacleWidth / 2);
            lastObstacleWidth = obstacleWidth;
        }
    }


    float GetValidSpawnPosition(float startX, float obstacleWidth)
    {
        float checkX = startX;
        bool positionIsFree = false;
        int maxAttempts = 6; // Evita bucles infinitos

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

                        // Si el nuevo obst�culo colisiona con otro, desplazamos su posici�n
                        if ((checkX >= leftEdge && checkX <= rightEdge) || (checkX + obstacleWidth >= leftEdge && checkX + obstacleWidth <= rightEdge))
                        {
                            positionIsFree = false;
                            checkX += 1f; // Desplazamos a la derecha
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
