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

            // Definir separaci�n m�nima y m�xima
            float minSeparation = 0.1f; // Casi pegados
            float maxSeparation = 1.5f; // No m�s de 1.5f de separaci�n

            // Calcular una separaci�n dentro del rango permitido
            float separation = Random.Range(minSeparation, maxSeparation);

            // Nueva posici�n en X: Justo despu�s del �ltimo obst�culo + separaci�n
            float newX = lastObstaclePositionX + (lastObstacleWidth / 2) + (obstacleWidth / 2) + separation;

            // Ajustar posici�n para evitar superposiciones y a�adir separaci�n si es necesario
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
        int maxAttempts = 15; // Evita bucles infinitos
        float extraSeparation = 2f; // Si hay solapamiento fuera de c�mara, aumenta la separaci�n

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

                        // Si el nuevo obst�culo colisiona con otro, lo desplazamos m�s lejos
                        if ((checkX >= leftEdge && checkX <= rightEdge) || (checkX + obstacleWidth >= leftEdge && checkX + obstacleWidth <= rightEdge))
                        {
                            positionIsFree = false;
                            checkX += extraSeparation; // Aumentamos la separaci�n si est�n fuera de la c�mara
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
