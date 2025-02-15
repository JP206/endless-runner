using UnityEngine;

public class DecoManager : MonoBehaviour
{
    int foliageSpawnRate;
    DecoPool pool;
    float foliageTime = 0;

    void Start()
    {
        pool = GetComponent<DecoPool>();
        foliageSpawnRate = Random.Range(3, 6);
    }

    void Update()
    {
        if (foliageTime >= foliageSpawnRate)
        {
            foliageTime = 0;
            foliageSpawnRate = Random.Range(8, 15);
            SpawnFoliage();
        }

        foliageTime += Time.deltaTime;
    }

    void SpawnFoliage()
    {
        GameObject foliage = pool.GetPooledFoliage();
        if (foliage)
        {
            foliage.transform.position = new Vector3(transform.position.x, foliage.GetComponent<Obstacle>().GetPosY(), 0);
        }
    }
}
