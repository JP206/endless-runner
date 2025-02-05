using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    [SerializeField] float spawnRate;

    float time = 0;

    void Update()
    {
        if (time >= spawnRate)
        {
            time = 0;
            Instantiate(enemy, transform.position, Quaternion.identity);
        }

        time += Time.deltaTime;
    }
}
