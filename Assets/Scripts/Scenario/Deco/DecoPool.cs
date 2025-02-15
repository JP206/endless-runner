using System.Collections.Generic;
using UnityEngine;

public class DecoPool : MonoBehaviour
{
    List<GameObject> foliagePool;
    [SerializeField] GameObject[] foliagePrefabs;
    [SerializeField] int foliagePoolSize;

    void Start()
    {
        InitializeFoliagePool();
    }

    void InitializeFoliagePool()
    {
        foliagePool = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < foliagePrefabs.Length; i++)
        {
            for (int j = 0; j < foliagePoolSize; j++)
            {
                tmp = Instantiate(foliagePrefabs[i]);
                tmp.SetActive(false);
                foliagePool.Add(tmp);
            }
        }
    }

    public GameObject GetPooledFoliage()
    {
        int random = Random.Range(0, foliagePool.Count);
        while (foliagePool[random].activeSelf)
        {
            random = Random.Range(0, foliagePool.Count);
        }
        foliagePool[random].SetActive(true);
        return foliagePool[random];
    }
}
