using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float posY;

    void Update()
    {
        if (transform.position.x < -15)
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
