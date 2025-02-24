using System.Collections.Generic;
using UnityEngine;

public class ScenarioPool : MonoBehaviour
{
    List<GameObject> pooledMountains, pooledRocks;

    [Header("Prefabs")]
    [SerializeField] GameObject mountainPrefab; 
    [SerializeField] GameObject rockPrefab;

    [Header("Pool configuration")]
    [SerializeField] int mountainPoolSize;
    [SerializeField] int rockPoolSize;

    void Start()
    {
        InitializeMountains();
        InitializeRocks();
    }

    void InitializeMountains()
    {
        pooledMountains = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < mountainPoolSize; i++)
        {
            tmp = Instantiate(mountainPrefab);
            tmp.GetComponent<Parallax>().SetPool(this);
            tmp.SetActive(false);
            pooledMountains.Add(tmp);
        }

        pooledMountains[0].SetActive(true);
        pooledMountains[0].GetComponent<Parallax>().PoolInitialize();
    }

    void InitializeRocks()
    {
        pooledRocks = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < rockPoolSize; i++)
        {
            tmp = Instantiate(rockPrefab);
            tmp.GetComponent<Parallax>().SetPool(this);
            tmp.SetActive(false);
            pooledRocks.Add(tmp);
        }

        pooledRocks[0].SetActive(true);
        pooledRocks[0].GetComponent<Parallax>().PoolInitialize();
    }

    GameObject GetPooledMountain()
    {
        for (int i = 0; i < pooledMountains.Count; i++)
        {
            if (!pooledMountains[i].activeInHierarchy)
            {
                pooledMountains[i].SetActive(true);
                pooledMountains[i].GetComponent<Parallax>().PoolSpawn();
                return pooledMountains[i];
            }
        }
        return null;
    }

    GameObject GetPooledRock()
    {
        for (int i = 0; i < pooledRocks.Count; i++)
        {
            if (!pooledRocks[i].activeInHierarchy)
            {
                pooledRocks[i].SetActive(true);
                pooledRocks[i].GetComponent<Parallax>().PoolSpawn();
                return pooledRocks[i];
            }
        }
        return null;
    }

    public void GetPooledObject(ParallaxObject parallaxObject)
    {
        switch (parallaxObject)
        {
            case ParallaxObject.Mountain:
                GetPooledMountain();
                break;
            case ParallaxObject.Rock:
                GetPooledRock();
                break;
        }
    }
}
