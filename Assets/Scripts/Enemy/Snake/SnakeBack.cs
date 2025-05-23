using UnityEngine;

public class SnakeBack : MonoBehaviour
{
    private GroundedEnemy parent;

    private void Start()
    {
        parent = GetComponentInParent<GroundedEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parent?.Die();

        }
    }
}
