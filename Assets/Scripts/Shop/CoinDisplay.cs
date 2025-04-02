using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    private void Update()
    {
        coinText.text = "Coins: " + PlayerData.Coins;
    }
}
