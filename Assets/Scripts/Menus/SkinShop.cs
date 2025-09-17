using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinShop : MonoBehaviour
{
    [Header("UI References")]
    public GameObject[] buyButtons;         // Buttons to buy skins
    public TextMeshProUGUI[] skinPrices;    // Cost text for each skin
    public SkinManager sm;    // To change the brick skin
    
    [Header("Sprite Database")]
    public SpriteDatabase database;

    public TextMeshProUGUI[] skinNames;

    public CoinUI coinUI;

    private SkinData[] skins;

    void OnEnable()
    {
        if (PlayerDataManager.Instance == null || PlayerDataManager.Instance.data == null)
        {
            Debug.LogError("PlayerDataManager not initialized!");
            return;
        }

        skins = PlayerDataManager.Instance.data.skins;

        SetupButtons();
        RefreshShopUI();
    }

    /// Automatically assign OnClick listeners to buttons with correct skin indices
    private void SetupButtons()
    {
        if (buyButtons == null || skins == null)
            return;

        int buttonCount = Mathf.Min(buyButtons.Length, skins.Length - 1); // skip default skin (index 0)

        for (int i = 0; i < buttonCount; i++)
        {
            int skinIndex = i + 1; // +1 to skip default skin
            Button btn = buyButtons[i].GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => BuySkin(skinIndex));
            }
        }

        //SkinNames Texts set based on database

        for (int i = 0; i < skinNames.Length; i++)
        {
            skinNames[i].text = database.skinNames[i+1];
        }
    }

    /// Refreshes button states and price texts
    public void RefreshShopUI()
    {
        if (skins == null || buyButtons == null || skinPrices == null)
            return;

        int buttonCount = Mathf.Min(buyButtons.Length, skins.Length - 1);

        for (int i = 0; i < buttonCount; i++)
        {
            int skinIndex = i + 1; // skip default skin
            SkinData skin = skins[skinIndex];

            Button btn = buyButtons[i].GetComponent<Button>();
            TextMeshProUGUI btnText = buyButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI priceText = i < skinPrices.Length ? skinPrices[i] : null;

            if (skin.owned)
            {
                if (btn != null) btn.interactable = false;
                if (btnText != null) btnText.text = "Owned";
                if (priceText != null) priceText.gameObject.SetActive(false);
            }
            else
            {
                if (btn != null) btn.interactable = true;
                if (btnText != null) btnText.text = "Buy";
                if (priceText != null)
                {
                    priceText.gameObject.SetActive(true);
                    priceText.text = "Cost: " + skin.price;
                }
            }
        }
    }

    /// Buys a skin safely
    public void BuySkin(int index)
    {
        var data = PlayerDataManager.Instance?.data;

        if (data == null)
        {
            Debug.LogError("PlayerDataManager not initialized!");
            return;
        }

        if (data.skins == null || index < 0 || index >= data.skins.Length)
        {
            Debug.LogError($"Invalid skin index {index}");
            return;
        }

        // Always work directly on the array reference
        if (!data.skins[index].owned && data.coins >= data.skins[index].price)
        {
            data.coins -= data.skins[index].price;
            data.skins[index].owned = true;

            // Only this one
            PlayerDataManager.Instance.Save();

            // Update UI + equipped skin
            RefreshShopUI();
            coinUI.UpdateCoins();
            //sm.ChangeBrickSkin(index);

            Debug.Log("Skin purchased, owned: " + data.skins[index].owned);
        }

        else if (data.coins < data.skins[index].price)
        {
            Debug.Log("Not enough coins to buy this skin!");
        }
    }
}

