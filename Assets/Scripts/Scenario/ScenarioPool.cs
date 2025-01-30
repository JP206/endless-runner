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

    [Header("Objects starting position")]
    [SerializeField] float startPosXMountain; 
    [SerializeField] float startPosYMountain, startPosXRock, startPosYRock;

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
        pooledMountains[0].transform.position = new Vector3(startPosXMountain, startPosYMountain, 0);
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
        pooledRocks[0].transform.position = new Vector3(startPosXRock, startPosYRock, 0);
    }

    public GameObject GetPooledMountain()
    {
        for (int i = 0; i < pooledMountains.Count; i++)
        {
            if (!pooledMountains[i].activeInHierarchy)
            {
                pooledMountains[i].SetActive(true);
                return pooledMountains[i];
            }
        }
        return null;
    }

    public GameObject GetPooledRock()
    {
        for (int i = 0; i < pooledRocks.Count; i++)
        {
            if (!pooledRocks[i].activeInHierarchy)
            {
                pooledRocks[i].SetActive(true);
                return pooledRocks[i];
            }
        }
        return null;
    }
}
