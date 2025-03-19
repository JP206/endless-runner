using UnityEngine;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private int spawnRate;
    [SerializeField] private float spawnOffset;
    [SerializeField] private float triggerDistance = 5f;
    [SerializeField] private Camera cameraXPos;

    private GameObject lastSpawnedObstacle;
    private ObstaclePool pool;
    private float lastObstaclePositionX = 0;
    private float referenceX = 0; // Punto de referencia que avanza con el entorno
    private float nextTriggerX = 0; // Ahora será exactamente donde termina el último collider

    void Start()
    {
        pool = Object.FindFirstObjectByType<ObstaclePool>();

        if (pool == null)
        {
            Debug.LogError("ObstaclePool no encontrado. Asegúrate de que hay un ObstaclePool en la escena.");
            return;
        }

        referenceX = transform.position.x;
        lastObstaclePositionX = referenceX;

        // Generamos el primer obstáculo al inicio
        SpawnObstacle();

        // 🚀 Ajustamos `nextTriggerX` después del primer spawn
        if (nextTriggerX < referenceX)
        {
            nextTriggerX = referenceX + 5f; // Ajustamos con un margen de seguridad
        }
    }

    void Update()
    {
        referenceX += Time.deltaTime * 5f; // Simulamos el avance del entorno

        // Si el punto de referencia ha alcanzado el trigger, generamos el siguiente prefab
        if (referenceX >= nextTriggerX)
        {
            SpawnObstacle();
        }
    }

    void SpawnObstacle()
    {
        if (pool == null) return;

        GameObject obstacle = pool.GetRandomObstacle();
        if (obstacle == null) return;

        // 📌 Encontrar el Collider más a la derecha del último obstáculo
        Collider2D lastRightmostCollider = lastSpawnedObstacle != null ? GetRightmostCollider(lastSpawnedObstacle) : null;
        float lastRightmostX = lastRightmostCollider != null ? lastRightmostCollider.bounds.max.x : lastObstaclePositionX;

        // 📌 Encontrar el primer Collider2D del nuevo prefab
        Collider2D firstCollider = GetFirstCollider(obstacle);
        if (firstCollider == null)
        {
            Debug.LogWarning($"⚠️ El prefab {obstacle.name} no tiene Colliders.");
            return;
        }

        // ✅ Ahora tomamos el punto más a la izquierda del nuevo prefab
        float firstColliderX = firstCollider.bounds.min.x;

        // 📌 Calculamos la distancia necesaria para mover el nuevo prefab
        float distanceToMove = (lastRightmostX + 5f) - firstColliderX;

        // 📌 Movemos el nuevo prefab para alinearlo correctamente con un margen de 5 unidades
        obstacle.transform.position += new Vector3(distanceToMove, 0, 0);
        obstacle.SetActive(true);

        // ✅ Actualizamos el siguiente punto de activación basado en el nuevo prefab generado
        lastSpawnedObstacle = obstacle;
        lastObstaclePositionX = lastRightmostX;
        nextTriggerX = lastRightmostX + triggerDistance + 5f;
    }

    Collider2D GetRightmostCollider(GameObject obj)
    {
        Collider2D[] colliders = obj.GetComponentsInChildren<Collider2D>();

        if (colliders.Length == 0)
        {
            return null;
        }

        float rightmostX = float.MinValue;
        Collider2D rightmostCollider = null;

        foreach (var col in colliders)
        {
            float colliderRightEdge = col.bounds.max.x;

            if (colliderRightEdge > rightmostX)
            {
                rightmostX = colliderRightEdge;
                rightmostCollider = col;
            }
        }

        if (rightmostCollider != null)
        {
            GameObject rightmostObject = rightmostCollider.gameObject;
            float objectX = rightmostObject.transform.position.x;
        }

        return rightmostCollider;
    }

    Collider2D GetFirstCollider(GameObject obj)
    {
        Collider2D[] colliders = obj.GetComponentsInChildren<Collider2D>();

        if (colliders.Length == 0) return null;

        Collider2D firstCollider = colliders[0];
        float leftmostX = firstCollider.bounds.min.x;

        foreach (var col in colliders)
        {
            if (col.bounds.min.x < leftmostX)
            {
                leftmostX = col.bounds.min.x;
                firstCollider = col;
            }
        }

        if (firstCollider != null)
        {
            GameObject firstObject = firstCollider.gameObject;
            float objectX = firstObject.transform.position.x;
        }

        return firstCollider;
    }
}
