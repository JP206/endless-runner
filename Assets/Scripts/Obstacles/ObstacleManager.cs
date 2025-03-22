using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private int spawnRate;
    [SerializeField] private float spawnOffset;
    [SerializeField] private float triggerDistance = 5f;
    [SerializeField] private Camera cameraXPos;

    private GameObject firstSpawnedObstacle;
    private GameObject lastSpawnedObstacle;
    private ObstaclePool pool;
    private float lastObstaclePositionX = 0;
    private float referenceX = 0;
    private float nextTriggerX = 0;

    float camMinX => Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x;

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

        SpawnObstacle();

        if (nextTriggerX < referenceX)
        {
            nextTriggerX = referenceX + 5f;
        }
    }

    void Update()
    {
        referenceX += Time.deltaTime * 5f;

        if (referenceX >= nextTriggerX)
        {
            referenceX = 0;
            SpawnObstacle();
        }
    }

    void SpawnObstacle()
    {
        Debug.Log("INSIDE SPAWN OBSTACLE");
        if (pool == null) return;

        GameObject obstacle = pool.GetRandomObstacle();
        if (obstacle == null) return;

        Collider2D lastRightmostCollider = lastSpawnedObstacle != null ? GetRightmostCollider(lastSpawnedObstacle) : null;
        float lastRightmostX = lastRightmostCollider != null ? lastRightmostCollider.bounds.max.x : lastObstaclePositionX;

        Collider2D firstCollider = GetFirstCollider(obstacle);
        if (firstCollider == null)
        {
            return;
        }

        float firstColliderX = firstCollider.bounds.min.x;

        float distanceToMove = (lastRightmostX + 3f) - firstColliderX;

        obstacle.transform.position += new Vector3(distanceToMove, 0, 0);
        obstacle.SetActive(true);

        Debug.Log("referenceX: " + referenceX);
        Debug.Log("nextTriggerX: " + nextTriggerX);

        firstSpawnedObstacle = lastSpawnedObstacle;
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

    public void CheckCameraBorder()
    {
        if(firstSpawnedObstacle.transform.position.x > camMinX)
        {
            StartCoroutine(TimeCondition());
        }
    }

    IEnumerator TimeCondition()
    {
        yield return new WaitForSeconds(2f);
        firstSpawnedObstacle.SetActive(false);
    }
}
