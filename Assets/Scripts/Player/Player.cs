using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float jumpForce;
    ProyectilePool pool;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pool = GetComponent<ProyectilePool>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            GameObject proyectile = pool.GetPooledObject();
            if (proyectile)
            {
                proyectile.transform.position = transform.position;
            }
        }
    }
}
