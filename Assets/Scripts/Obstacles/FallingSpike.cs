using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    [SerializeField] float posX, posY;

    void Update()
    {
        if (transform.position.x <= -1.5f)
        {
            transform.position -= new Vector3(0, 2.5f * Time.deltaTime, 0);
        }
        else
        {
            transform.localPosition = new Vector3(posX, posY, 0);
        }
    }
}
