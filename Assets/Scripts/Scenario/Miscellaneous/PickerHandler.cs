using UnityEngine;
using System.Collections;

public class PickerHandler : MonoBehaviour
{
    float duration = 0.6f;
    float elapsed = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dino Leg") || collision.CompareTag("Coin"))
        {
            StartCoroutine(AnimatePickup(collision.gameObject));
        }
    }

    IEnumerator AnimatePickup(GameObject pickup)
    {

        Vector3 originalPos = pickup.transform.position;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            pickup.transform.Rotate(0f, 1440f * Time.deltaTime, 0f);

            float height = Mathf.Sin((elapsed / duration) * Mathf.PI) * 0.5f;
            pickup.transform.position = originalPos + new Vector3(0, height, 0);

            yield return null;
        }

        Destroy(pickup);
    }

}
