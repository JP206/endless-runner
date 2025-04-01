using UnityEngine;

public class EnemyBack : MonoBehaviour
{
    private Enemy parentEnemy;

    private void Start()
    {
        parentEnemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parentEnemy.SetVulnerability(false); 
            parentEnemy.DestroyEnemy();
        }
    }
}
