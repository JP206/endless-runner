using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    List<GameObject> pooledObstacles;
    [SerializeField] GameObject[] obstaclePrefabs;
    [SerializeField] int poolSize;

    void Start()
    {
        InitializePool();
    }

    void InitializePool()
    {
        pooledObstacles = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < obstaclePrefabs.Length; i++)
        {
            for (int j = 0; j < poolSize; j++)
            {
                tmp = Instantiate(obstaclePrefabs[i]);
                tmp.SetActive(false);
                pooledObstacles.Add(tmp);
            }
        }
    }

    public GameObject GetPooledObstacle()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!pooledObstacles[i].activeInHierarchy)
            {
                pooledObstacles[i].SetActive(true);
                return pooledObstacles[i];
            }
        }
        return null;
    }

    public GameObject GetRandomObstacle()
    {
        int random = Random.Range(0, pooledObstacles.Count);
        GameObject result = pooledObstacles[random];

        while (result.activeInHierarchy)
        {
            random = Random.Range(0, pooledObstacles.Count);
            result = pooledObstacles[random];
        }

        result.SetActive(true);
        return result;
    }
}
