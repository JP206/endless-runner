using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    float spawnRate, time = 0;

    void Start()
    {
        spawnRate = Random.Range(2, 5);
    }

    void Update()
    {
        if (time >= spawnRate)
        {
            spawnRate = Random.Range(10, 20);
            time = 0;
            Instantiate(enemy, transform.position, Quaternion.identity);
        }

        time += Time.deltaTime;
    }
}
