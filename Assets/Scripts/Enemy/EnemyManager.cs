using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    float time = 0;

    void Update()
    {
        if (time >= 3)
        {
            time = 0;
            Instantiate(enemy, transform.position, Quaternion.identity);
        }

        time += Time.deltaTime;
    }
}
