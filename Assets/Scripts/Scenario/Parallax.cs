using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] ParallaxObject parallaxObject;

    ScenarioPool pool;
    SpriteRenderer spriteRenderer = null;
    bool spawnFlag = false;
    
    float camMaxX => Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0f, 0f)).x;
    float camMinX => Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x;

    void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        transform.Translate(Time.deltaTime * speed * Vector2.left);
    
        if (spriteRenderer.bounds.max.x <= camMaxX)
        {
            if (!spawnFlag)
            {
                spawnFlag = true;
                pool.GetPooledObject(parallaxObject);
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

    public void PoolSpawn()
    {
        transform.position = new Vector3(camMaxX + spriteRenderer.bounds.extents.x, transform.position.y, 0);
    }

    public void PoolInitialize()
    {
        transform.position = new Vector3(camMinX + spriteRenderer.bounds.extents.x, transform.position.y, 0);
    }
}

public enum ParallaxObject
{
    Mountain,
    Rock
}
