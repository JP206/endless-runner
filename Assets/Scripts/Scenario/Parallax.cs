using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float speed;
    SpriteRenderer spriteRenderer;
    float camMaxX => Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0f, 0f)).x;
    float camMinX => Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector2.left);
    
        if (spriteRenderer.bounds.max.x <= camMaxX)
        {
            Vector3 spawnPoint = new Vector3(transform.position.x + spriteRenderer.bounds.extents.x * 2, transform.position.y, 0);
            Instantiate(gameObject, spawnPoint, Quaternion.identity);
        }

        if (spriteRenderer.bounds.max.x <= camMinX)
        {
            Destroy(gameObject);
        }
    }
}
