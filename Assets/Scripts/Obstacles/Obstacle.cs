using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private const string TIGGER_LAYER_NAME = "Tigger";
    void Update()
    {
        int layerIndex = LayerMask.NameToLayer(TIGGER_LAYER_NAME);
        gameObject.layer = layerIndex;

        if (transform.position.x < -10)
        {
            Destroy(gameObject);
        }

        transform.position -= new Vector3(5 * Time.deltaTime, 0, 0);
    }
}
