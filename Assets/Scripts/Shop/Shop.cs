using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Audio;

public class Shop : MonoBehaviour
{
    [Header("Canvas References")]
    [SerializeField] private GameObject shopCanvas;
    [SerializeField] private GameObject addsCanvas;

    [Header("Payed Shop Buttons")]
    [SerializeField] private Button extraLifeButton;
    [SerializeField] private Button dinoLegButton;
    [SerializeField] private Button invincibleButton;

    [Header("Shop Containers")]
    [SerializeField] private Image extraLifeContainer;
    [SerializeField] private Image dinoLegContainer;
    [SerializeField] private Image invincibleContainer;

    [Header("AddsCanvas Container")]
    [SerializeField] private Image extraLifeAddContainer;
    [SerializeField] private Image tripleAttackAddContainer;
    [SerializeField] private Image doublePointsAddContainer;
    [SerializeField] private Image goBackAddImage;
    [SerializeField] private Image homeImage;

    [Header("Other Buttons")]
    [SerializeField] private Image goBackImage;
    [SerializeField] private Image addsTabImage;

    [Header("Disabled Sprites")]
    [SerializeField] private Sprite extraLifeDisabledSprite;
    [SerializeField] private Sprite dinoLegDisabledSprite;
    [SerializeField] private Sprite invincibleDisabledSprite;

    [Header("Normal Sprites")]
    [SerializeField] private Sprite extraLifeNormalSprite;
    [SerializeField] private Sprite dinoLegNormalSprite;
    [SerializeField] private Sprite invincibleNormalSprite;

    [Header("Sounds Click")]
    [SerializeField] private AudioSource clickSound;

    private GameObject currentCanvas;

    void Start()
    {
        currentCanvas = shopCanvas;
        UpdateUI();
    }

    public void OpenTab(GameObject canvasToOpen)
    {
        ClickButtonSound();
        shopCanvas.SetActive(false);
        addsCanvas.SetActive(false);

        canvasToOpen.SetActive(true);
        currentCanvas = canvasToOpen;

        if (canvasToOpen == addsCanvas && addsTabImage != null)
        {
            AnimateButtonPress(addsTabImage.transform);
        }
        UpdateUI();
    }

    public void CloseTab(GameObject canvasToClose)
    {
        canvasToClose.SetActive(false);
        shopCanvas.SetActive(true);
        currentCanvas = shopCanvas;

        AnimateButtonPress(goBackImage.transform);
        UpdateUI();
    }

    public void LoadMainMenu()
    {
        ClickButtonSound();
        SceneManager.LoadScene("MainMenu");
    }

    public void Purchase(string selectedTabName)
    {
        if (currentCanvas == shopCanvas)
        {
            switch (selectedTabName)
            {
                case "ExtraLife":
                    AnimateButtonPress(extraLifeContainer.transform);
                    TryPurchase(500, "ExtraLife");
                    break;
                case "DinoLeg":
                    AnimateButtonPress(dinoLegContainer.transform);
                    TryPurchase(1000, "DinoLeg");
                    break;
                case "Invincible":
                    AnimateButtonPress(invincibleContainer.transform);
                    TryPurchase(2000, "Invincible");
                    break;
                default:
                    break;
            }
        }
        else if (currentCanvas == addsCanvas)
        {
            Transform target = selectedTabName switch
            {
                "ExtraLife" => extraLifeAddContainer.transform,
                "TripleAttack" => tripleAttackAddContainer.transform,
                "DoublePoints" => doublePointsAddContainer.transform,
                "GoBack" => goBackAddImage.transform,
                "Home" => homeImage.transform,
                _ => null
            };

            if (target != null) AnimateButtonPress(target);

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
            PlayerPrefs.SetInt("coins", PlayerData.Coins);
            int itemStackCount = PlayerPrefs.GetInt(itemName);
            if (itemStackCount < 10)
            {
                PlayerPrefs.SetInt(itemName, itemStackCount + 1);
            }
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
                Debug.Log("No se completó el anuncio, no hay recompensa.");
            }
        };
        PokiUnitySDK.Instance.rewardedBreak();
    }

    private void AnimateButtonPress(Transform target)
    {
        StartCoroutine(ButtonPressAnimation(target));
    }

    private IEnumerator ButtonPressAnimation(Transform target)
    {
        ClickButtonSound();
        Vector3 originalScale = target.localScale;
        Vector3 pressedScale = originalScale * 0.95f;

        float duration = 0.05f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            target.localScale = Vector3.Lerp(originalScale, pressedScale, elapsed / duration);
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            target.localScale = Vector3.Lerp(pressedScale, originalScale, elapsed / duration);
            yield return null;
        }

        target.localScale = originalScale;
    }

    public void ClickButtonSound()
    {
        if (clickSound != null && clickSound.clip != null)
            clickSound.PlayOneShot(clickSound.clip);
    }
}