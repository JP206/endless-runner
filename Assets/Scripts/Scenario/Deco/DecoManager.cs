using UnityEngine;

public class DecoManager : MonoBehaviour
{
    int foliageSpawnRate;
    DecoPool pool;
    float foliageTime = 0;

    float lastFoliagePositionX = 0;
    float lastFoliageWidth = 0;

    void Start()
    {
        pool = GetComponent<DecoPool>();
        foliageSpawnRate = Random.Range(3, 6);
    }

    void Update()
    {
        if (foliageTime >= foliageSpawnRate)
        {
            foliageTime = 0;
            foliageSpawnRate = Random.Range(8, 15);
            SpawnFoliage();
        }

        foliageTime += Time.deltaTime;
    }

    void SpawnFoliage()
    {
        GameObject foliage = pool.GetPooledFoliage();
        if (foliage)
        {
            float foliageWidth = GetFoliageWidth(foliage);
            float minSeparation = 0.2f;
            float maxSeparation = 2f; 
            float separation = Random.Range(minSeparation, maxSeparation);

            float newX = lastFoliagePositionX + (lastFoliageWidth / 2) + (foliageWidth / 2) + separation;

            newX = GetValidFoliagePosition(newX, foliageWidth);

            //foliage.transform.position = new Vector3(newX, foliage.GetComponent<Obstacle>().GetPosY(), 0);

            lastFoliagePositionX = newX + (foliageWidth / 2);
            lastFoliageWidth = foliageWidth;
        }
    }

    float GetFoliageWidth(GameObject foliage)
    {
        Collider2D col = foliage.GetComponent<Collider2D>();
        if (col != null)
        {
            return col.bounds.size.x;
        }

        SpriteRenderer sprite = foliage.GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            return sprite.bounds.size.x;
        }

        return 3f;
    }

    float GetValidFoliagePosition(float startX, float foliageWidth)
    {
        float checkX = startX;
        bool positionIsFree = false;
        int maxAttempts = 10;
        float extraSeparation = 2f;

        while (!positionIsFree && maxAttempts > 0)
        {
            positionIsFree = true;
            foreach (var existingFoliage in pool.GetAllFoliage())
            {
                if (existingFoliage.activeInHierarchy)
                {
                    Collider2D col = existingFoliage.GetComponent<Collider2D>();
                    if (col != null)
                    {
                        float leftEdge = col.bounds.min.x;
                        float rightEdge = col.bounds.max.x;

                        if ((checkX >= leftEdge && checkX <= rightEdge) || (checkX + foliageWidth >= leftEdge && checkX + foliageWidth <= rightEdge))
                        {
                            positionIsFree = false;
                            checkX += extraSeparation;
                            break;
                        }
                    }
                }
            }
            maxAttempts--;
        }
        return checkX;
    }
}
