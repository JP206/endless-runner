using UnityEngine;
using System.Collections;

public class PlayerOutOfBounds : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private float delayBeforeGameOver = 0.05f;

    private bool isGameOverTriggered = false;

    private void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        if (playerHealth == null) playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (isGameOverTriggered) return;

        Vector3 playerPosition = transform.position;
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(playerPosition);

        if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0)
        {
            StartCoroutine(DelayedGameOver()); 
        }
    }

    private IEnumerator DelayedGameOver()
    {
        if (isGameOverTriggered) yield break;

        isGameOverTriggered = true;

        yield return new WaitForSeconds(delayBeforeGameOver);

        if (playerHealth != null) playerHealth.Die();
    }
}
