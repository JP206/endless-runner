using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    List<GameObject> pooledEnemies;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] int poolSize;

    void Start()
    {
        pooledEnemies = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < poolSize; i++)
        {
            tmp = Instantiate(enemyPrefab);
            tmp.SetActive(false);
            pooledEnemies.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!pooledEnemies[i].activeInHierarchy)
            {
                pooledEnemies[i].SetActive(true);
                return pooledEnemies[i];
            }
        }
        return null;
    }
}
