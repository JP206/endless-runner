using UnityEngine;

public class DecoManager : MonoBehaviour
{
    int foliageSpawnRate;
    DecoPool pool;
    float foliageTime = 0;

    float lastFoliagePositionX = 0; // �ltima posici�n en X de una decoraci�n
    float lastFoliageWidth = 0; // Ancho de la �ltima decoraci�n

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

            // Definir separaci�n m�nima y m�xima
            float minSeparation = 0.2f; // Casi pegados
            float maxSeparation = 2f; // No m�s de 2f de separaci�n para evitar huecos grandes

            // Calcular una separaci�n dentro del rango permitido
            float separation = Random.Range(minSeparation, maxSeparation);

            // Calcular la nueva posici�n en X siguiendo la �ltima decoraci�n generada
            float newX = lastFoliagePositionX + (lastFoliageWidth / 2) + (foliageWidth / 2) + separation;

            // Ajustar posici�n para evitar superposiciones
            newX = GetValidFoliagePosition(newX, foliageWidth);

            // Asignar la nueva posici�n a la decoraci�n
            foliage.transform.position = new Vector3(newX, foliage.GetComponent<Obstacle>().GetPosY(), 0);

            // Guardar la nueva posici�n y ancho para la siguiente decoraci�n
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

        return 3f; // Valor por defecto si no tiene colisionador ni sprite
    }

    float GetValidFoliagePosition(float startX, float foliageWidth)
    {
        float checkX = startX;
        bool positionIsFree = false;
        int maxAttempts = 10; // Para evitar un bucle infinito
        float extraSeparation = 2f; // Si hay superposici�n, se separa m�s

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

                        // Si la decoraci�n colisiona con otra, la desplazamos
                        if ((checkX >= leftEdge && checkX <= rightEdge) || (checkX + foliageWidth >= leftEdge && checkX + foliageWidth <= rightEdge))
                        {
                            positionIsFree = false;
                            checkX += extraSeparation; // Separaci�n adicional
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
