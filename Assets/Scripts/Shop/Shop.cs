using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [Header("Canvas References")]
    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private GameObject addsCanvas;

    [Header("Botones del Shop (pagos)")]
    [SerializeField] private Button extraLifeButton;
    [SerializeField] private Button dinoLegButton;
    [SerializeField] private Button invincibleButton;

    private GameObject currentCanvas;

    void Start()
    {
        currentCanvas = shopCanvas;
        UpdateUI();
    }

    public void OpenTab(GameObject canvasToOpen)
    {
        shopCanvas.SetActive(false);
        addsCanvas.SetActive(false);

        canvasToOpen.SetActive(true);
        currentCanvas = canvasToOpen;

        UpdateUI();
    }

    public void CloseTab(GameObject canvasToClose)
    {
        canvasToClose.SetActive(false);
        shopCanvas.SetActive(true);
        currentCanvas = shopCanvas;

        UpdateUI();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Purchase(string selectedTabName)
    {
        if (currentCanvas == shopCanvas)
        {
            switch (selectedTabName)
            {
                case "ExtraLife":
                    TryPurchase(500, "ExtraLife");
                    break;
                case "DinoLeg":
                    TryPurchase(1000, "DinoLeg");
                    break;
                case "Invincible":
                    TryPurchase(2000, "Invincible");
                    break;
                default:
                    break;
            }
        }
        else if (currentCanvas == addsCanvas)
        {
            Debug.Log($"Mostrando anuncio para obtener: {selectedTabName}");
            PokiUnitySDK.Instance.rewardedBreakCallBack = (bool withReward) =>
            {
                Debug.Log($"withReward: {withReward}");
                if (withReward)
                {
                    Debug.Log($"¡Recibiste {selectedTabName} viendo un anuncio! ✨");
                }
                else
                {
                    Debug.Log("No se completó el anuncio, no hay recompensa.");
                }
            };
            PokiUnitySDK.Instance.rewardedBreak();
        }

        UpdateUI();
    }

    private void TryPurchase(int cost, string itemName)
    {
        if (PlayerData.Coins >= cost)
        {
            PlayerData.Coins -= cost;
            Debug.Log($"Compraste: {itemName}. Monedas restantes: {PlayerData.Coins}");
        }
        else
        {
            Debug.Log($"No tenés suficientes monedas para comprar: {itemName}. Te faltan {cost - PlayerData.Coins} monedas.");
        }
    }

    private void UpdateUI()
    {
        if (currentCanvas == shopCanvas)
        {
            extraLifeButton.interactable = PlayerData.Coins >= 500;
            dinoLegButton.interactable = PlayerData.Coins >= 1000;
            invincibleButton.interactable = PlayerData.Coins >= 2000;
        }
    }
}
