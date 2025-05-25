using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("Canvas References")]
    [SerializeField] private GameObject shopCanvas;

    private bool isShopOpen = false;

    void Start()
    {
        shopCanvas.SetActive(false);
    }

    void Update()
    {
        if (isShopOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShop();
        }
    }

    public void OpenShop()
    {
        shopCanvas.SetActive(true);
        isShopOpen = true;
    }

    public void CloseShop()
    {
        shopCanvas.SetActive(false);
        isShopOpen = false;
    }
}
