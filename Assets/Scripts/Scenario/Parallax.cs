using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] bool mountain;

    ScenarioPool pool;
    SpriteRenderer spriteRenderer;
    bool spawnFlag = false;
    
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
            if (mountain && !spawnFlag)
            {
                spawnFlag = true;
                GameObject mountainInstance = pool.GetPooledMountain();
                mountainInstance.transform.position = spawnPoint;
            }
            else if (!spawnFlag)
            {
                spawnFlag = true;
                GameObject mountainInstance = pool.GetPooledRock();
                mountainInstance.transform.position = spawnPoint;
            }
        }

        if (spriteRenderer.bounds.max.x <= camMinX)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetPool(ScenarioPool _pool)
    {
        pool = _pool;
    }

    private void OnDisable()
    {
        spawnFlag = false;
    }
}
