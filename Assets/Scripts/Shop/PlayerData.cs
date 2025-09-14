using UnityEngine;

public static class PlayerData
{
    public static int Coins { get; private set; }

    private static bool loaded = false;

    public static void LoadData()
    {
        if (loaded) return;
        loaded = true;

        Coins = PlayerPrefs.GetInt("coins", 0);
    }

    public static void SaveCoins()
    {
        PlayerPrefs.SetInt("coins", Coins);
    }

    public static void AddCoins(int amount)
    {
        Coins += amount;
        SaveCoins();
    }

    public static bool SpendCoins(int amount)
    {
        if (Coins >= amount)
        {
            Coins -= amount;
            SaveCoins();
            return true;
        }
        return false;
    }

    public static int ExtraLife => PlayerPrefs.GetInt("ExtraLife", 0);
    public static int DinoLeg => PlayerPrefs.GetInt("DinoLeg", 0);
    public static int Invincible => PlayerPrefs.GetInt("Invincible", 0);
    public static int TripleAttack => PlayerPrefs.GetInt("TripleAttack", 0);
    public static int DoublePoints => PlayerPrefs.GetInt("DoublePoints", 0);

    public static void AddItem(string itemName, int amount = 1)
    {
        int current = PlayerPrefs.GetInt(itemName, 0);
        int newAmount = Mathf.Min(current + amount, 10);
        PlayerPrefs.SetInt(itemName, newAmount);
    }

    public static bool UseItem(string itemName)
    {
        int current = PlayerPrefs.GetInt(itemName, 0);
        if (current > 0)
        {
            PlayerPrefs.SetInt(itemName, current - 1);
            return true;
        }
        return false;
    }

    public static int GetItemCount(string itemName)
    {
        return PlayerPrefs.GetInt(itemName, 0);
    }

    private static void SetItem(string itemName, int amount)
    {
        PlayerPrefs.SetInt(itemName, amount);
    }

    public static void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        Coins = 0;
        loaded = false;
    }
}
