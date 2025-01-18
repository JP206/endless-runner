using System.Collections.Generic;
using UnityEngine;

public class ProyectilePool : MonoBehaviour
{
    List<GameObject> pooledProyectiles;
    [SerializeField] GameObject proyectilePrefab;
    [SerializeField] int poolSize;

    void Start()
    {
        pooledProyectiles = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < poolSize; i++)
        {
            tmp = Instantiate(proyectilePrefab);
            tmp.SetActive(false);
            pooledProyectiles.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!pooledProyectiles[i].activeInHierarchy)
            {
                pooledProyectiles[i].SetActive(true);
                return pooledProyectiles[i];
            }
        }
        return null;
    }
}
