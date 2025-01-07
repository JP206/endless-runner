using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] GameObject obstacle;
    float time = 0;

    void Update()
    {
        if (time >= 3)
        {
            time = 0;
            Instantiate(obstacle, transform.position, Quaternion.identity);
        }

        time += Time.deltaTime;
    }
}
