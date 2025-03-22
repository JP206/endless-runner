using UnityEngine;

public class PrefabDeactivator : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    private ObstacleManager obstacleManager;
    private GameObject parentPrefab;

    void Start()
    {
        obstacleManager = Object.FindFirstObjectByType<ObstacleManager>();

        if (obstacleManager == null)
        {
            Debug.LogError("ObstacleManager no encontrado. Asegúrate de que está en la escena.");
        }

        parentPrefab = transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            if (obstacleManager != null)
            {
                obstacleManager.CheckCameraBorder();
            }
        }
    }
}
