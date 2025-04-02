using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [Header("Canvas References")]
    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private GameObject addsCanvas;

    [SerializeField] private Button extraLifeButton;
    [SerializeField] private Button dinoLegButton;
    [SerializeField] private Button invincibleButton;

    [SerializeField] private Image extraLifeContainer;
    [SerializeField] private Image dinoLegContainer;
    [SerializeField] private Image invincibleContainer;

    [SerializeField] private Sprite extraLifeDisabledSprite;
    [SerializeField] private Sprite dinoLegDisabledSprite;
    [SerializeField] private Sprite invincibleDisabledSprite;

    [SerializeField] private Sprite extraLifeNormalSprite;
    [SerializeField] private Sprite dinoLegNormalSprite;
    [SerializeField] private Sprite invincibleNormalSprite;

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
            PokiCall(selectedTabName);
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
    }

    private void UpdateUI()
    {
        if (currentCanvas == shopCanvas)
        {
            bool canBuyExtraLife = PlayerData.Coins >= 500;
            bool canBuyDinoLeg = PlayerData.Coins >= 1000;
            bool canBuyInvincible = PlayerData.Coins >= 2000;

            extraLifeButton.interactable = canBuyExtraLife;
            dinoLegButton.interactable = canBuyDinoLeg;
            invincibleButton.interactable = canBuyInvincible;

            extraLifeContainer.sprite = canBuyExtraLife ? extraLifeNormalSprite : extraLifeDisabledSprite;
            dinoLegContainer.sprite = canBuyDinoLeg ? dinoLegNormalSprite : dinoLegDisabledSprite;
            invincibleContainer.sprite = canBuyInvincible ? invincibleNormalSprite : invincibleDisabledSprite;
        }
    }

    private void PokiCall(string selectedTabName)
    {
        PokiUnitySDK.Instance.rewardedBreakCallBack = (bool withReward) =>
        {
            if (withReward)
            {
                Debug.Log($"Recibiste {selectedTabName} viendo un anuncio!");
            }
            else
            {
                Debug.Log("No se complet el anuncio, no hay recompensa.");
            }
        };
        PokiUnitySDK.Instance.rewardedBreak();
    }
}