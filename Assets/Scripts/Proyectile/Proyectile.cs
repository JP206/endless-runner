using UnityEngine;

public class Proyectile : MonoBehaviour
{
    void Update()
    {
        transform.position += new Vector3(5 * Time.deltaTime, 0, 0);

        if (transform.position.x > 10) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null) enemy.DestroyEnemy();

            gameObject.SetActive(false);
        }
    }
}
