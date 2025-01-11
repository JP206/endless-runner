using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    List<GameObject> pooledObstacles;
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] int poolSize;

    void Start()
    {
        pooledObstacles = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < poolSize; i++)
        {
            tmp = Instantiate(obstaclePrefab);
            tmp.SetActive(false);
            pooledObstacles.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
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
}
