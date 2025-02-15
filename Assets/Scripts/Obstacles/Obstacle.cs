using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float posY;
    [SerializeField] float xThreshold;

    void Update()
    {
        if (transform.position.x < xThreshold)
        {
            gameObject.SetActive(false);
        }

        transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
    }

    public float GetPosY()
    {
        return posY;
    }
}
