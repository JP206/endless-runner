using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioPool : MonoBehaviour
{
    List<GameObject> pooledMountains, pooledRocks, pooledFarMountains, pooledPlants, pooledClouds;

    [Header("Prefabs")]
    [SerializeField] GameObject mountainPrefab; 
    [SerializeField] GameObject rockPrefab;
    [SerializeField] GameObject farMountainPrefab;
    [SerializeField] GameObject[] frontPlants;
    [SerializeField] GameObject[] clouds;

    [Header("Pool configuration")]
    [SerializeField] int mountainPoolSize;
    [SerializeField] int rockPoolSize;
    [SerializeField] int farMountainPoolSize;
    [SerializeField] int plantsPoolSize;
    [SerializeField] int cloudPoolSize;

    [Header("Front plants")]
    [SerializeField] Vector3 plantSpawnPos;

    [Header("Clouds")]
    [SerializeField] float yRangeMin, yRangeMax, xSpawnPos;

    void Start()
    {
        InitializeMountains();
        InitializeRocks();
        InitializeFarMountains();
        InitializePlants();
        InitializeClouds();

        StartCoroutine(SpawnFrontPlants());
        StartCoroutine(SpawnClouds());
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

    void InitializeFarMountains()
    {
        pooledFarMountains = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < farMountainPoolSize; i++)
        {
            tmp = Instantiate(farMountainPrefab);
            tmp.GetComponent<Parallax>().SetPool(this);
            tmp.SetActive(false);
            pooledFarMountains.Add(tmp);
        }

        pooledFarMountains[0].SetActive(true);
        pooledFarMountains[0].GetComponent<Parallax>().PoolInitialize();
    }

    void InitializePlants()
    {
        pooledPlants = new List<GameObject>();
        GameObject tmp;
        for (int j = 0; j < frontPlants.Length; j++)
        {
            for (int i = 0; i < plantsPoolSize; i++)
            {
                tmp = Instantiate(frontPlants[j]);
                tmp.SetActive(false);
                pooledPlants.Add(tmp);
            }
        }
        
        pooledPlants[0].SetActive(true);
    }

    void InitializeClouds()
    {
        pooledClouds = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < cloudPoolSize; i++)
        {
            tmp = Instantiate(clouds[i]);
            tmp.SetActive(false);
            pooledClouds.Add(tmp);
        }

        pooledClouds[0].SetActive(true);
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

    GameObject GetPooledFarMountain()
    {
        for (int i = 0; i < pooledFarMountains.Count; i++)
        {
            if (!pooledFarMountains[i].activeInHierarchy)
            {
                pooledFarMountains[i].SetActive(true);
                pooledFarMountains[i].GetComponent<Parallax>().PoolSpawn();
                return pooledFarMountains[i];
            }
        }
        return null;
    }

    GameObject GetRandomPooledPlant()
    {
        List<GameObject> availablePlants = new();

        foreach (GameObject plant in pooledPlants)
        {
            if (!plant.activeInHierarchy)
            {
                availablePlants.Add(plant);
            }
        }

        if (availablePlants.Count > 0)
        {
            int randomIndex = Random.Range(0, availablePlants.Count);
            GameObject selectedPlant = availablePlants[randomIndex];
            selectedPlant.SetActive(true);
            return selectedPlant;
        }

        return null;
    }

    GameObject GetRandomPooledCloud()
    {
        List<GameObject> availableClouds = new();

        foreach (GameObject cloud in pooledClouds)
        {
            if (!cloud.activeInHierarchy)
            {
                availableClouds.Add(cloud);
            }
        }

        if (availableClouds.Count > 0)
        {
            int randomIndex = Random.Range(0, availableClouds.Count);
            GameObject selectedCloud = availableClouds[randomIndex];
            selectedCloud.SetActive(true);
            return selectedCloud;
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
            case ParallaxObject.FarMountain:
                GetPooledFarMountain();
                break;
        }
    }

    IEnumerator SpawnFrontPlants()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5, 10));
            GetRandomPooledPlant().transform.position = plantSpawnPos;
        }
    }

    IEnumerator SpawnClouds()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(6, 12));
            Vector3 cloudSpawnPos = new(xSpawnPos, Random.Range(yRangeMin, yRangeMax), 0);
            GameObject cloud = GetRandomPooledCloud();
            if (cloud)
            {
                cloud.transform.position = cloudSpawnPos;
            }
        }
    }
}
