using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    List<GameObject> pooledObstacles;
    [SerializeField] GameObject[] obstaclePrefabs;
    [SerializeField] int poolSize;

    void Awake()
    { 
        InitializePool();
    }

    void InitializePool()
    {
        pooledObstacles = new List<GameObject>();
        for (int i = 0; i < obstaclePrefabs.Length; i++)
        {
            for (int j = 0; j < poolSize; j++)
            {
                GameObject tmp = Instantiate(obstaclePrefabs[i]);
                tmp.SetActive(false);
                pooledObstacles.Add(tmp);
            }
        }
    }

    public GameObject GetRandomObstacle()
    {
        List<GameObject> availableObstacles = new List<GameObject>();

        foreach (var obstacle in pooledObstacles)
        {
            if (!obstacle.activeInHierarchy)
            {
                availableObstacles.Add(obstacle);
            }
        }

        if (availableObstacles.Count > 0)
        {
            int randomIndex = Random.Range(0, availableObstacles.Count);
            GameObject selectedObstacle = availableObstacles[randomIndex];

            selectedObstacle.SetActive(true);

            foreach (var component in selectedObstacle.GetComponentsInChildren<MonoBehaviour>(true))
            {
                var method = component.GetType().GetMethod("ResetState");
                if (method != null)
                {
                    method.Invoke(component, null);
                }
            }

            return selectedObstacle;
        }

        return null;
    }

}
