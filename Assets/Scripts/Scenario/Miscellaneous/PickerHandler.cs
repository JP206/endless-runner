using UnityEngine;
using System.Collections;

public class PickerHandler : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dino Leg") || collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
        }
    }
}
