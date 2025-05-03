using UnityEngine;

public class PickerHandler : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dino leg"))
        {
            Destroy(collision.gameObject);
        }

        else if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
        }
    }
}
