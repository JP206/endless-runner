using UnityEngine;

public class EnemyBack : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GroundedEnemy parent = GetComponentInParent<GroundedEnemy>();
            if (parent != null)
            {
                parent.Die();
            }
        }
    }

}
