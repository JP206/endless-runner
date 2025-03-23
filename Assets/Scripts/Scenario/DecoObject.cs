using UnityEngine;

public class DecoObject : MonoBehaviour
{
    [SerializeField] float speed, thresholdX;

    void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector2.left);

        if (transform.position.x < thresholdX)
        {
            gameObject.SetActive(false);
        }
    }
}
