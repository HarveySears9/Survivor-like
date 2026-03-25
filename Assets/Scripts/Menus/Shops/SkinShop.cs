using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SkinShop : MonoBehaviour
{
    [Header("UI References")]
    public Transform shopContentParent;       // The ScrollView content panel
    public GameObject shopItemPrefab;         // Prefab for each skin item

    public SkinManager sm;
    public SpriteDatabase database;
    public CoinUI coinUI;
    public MenuDialogManager mdm;

    private SkinData[] skins;
    private List<GameObject> spawnedShopItems = new List<GameObject>();
    private Dictionary<int, Sprite> skinIcons;


    void Start()
    {
        ResizeContent(shopContentParent);
    }

    void OnEnable()
    {
        if (PlayerDataManager.Instance == null || PlayerDataManager.Instance.data == null)
        {
            Debug.LogError("PlayerDataManager not initialized!");
            return;
        }

        skins = PlayerDataManager.Instance.data.skins;

        BuildSkinIconDictionary();
        RefreshShopUI();
    }

    private void BuildSkinIconDictionary()
    {
        skinIcons = new Dictionary<int, Sprite>
        {
            { 0, database.red.Length > 0 ? database.red[0] : null },
            { 1, database.blue.Length > 0 ? database.blue[0] : null },
            { 2, database.black.Length > 0 ? database.black[0] : null },
            { 3, database.gold.Length > 0 ? database.gold[0] : null },
            { 4, database.teal.Length > 0 ? database.teal[0] : null },
            { 5, database.bone.Length > 0 ? database.bone[0] : null },
            { 6, database.suit.Length > 0 ? database.suit[0] : null },
            { 7, database.suit.Length > 0 ? database.blackBone[0] : null }
        };
    }

    /// Clears and rebuilds the shop UI
    public void RefreshShopUI()
    {
        // Clear old items
        foreach (var go in spawnedShopItems)
            Destroy(go);
        spawnedShopItems.Clear();

        if (skins == null) return;

        for (int i = 0; i < skins.Length; i++)
        {
            SkinData skin = skins[i];

            // Skip achievement-only skins
            if (skin.achievement)
                continue;

            GameObject instance = Instantiate(shopItemPrefab, shopContentParent);
            spawnedShopItems.Add(instance);

            ShopItemUI ui = instance.GetComponent<ShopItemUI>();

            if (ui != null)
            {
                // Set name from database (if exists)
                if (i < database.skinNames.Length)
                    ui.skinNameText.text = database.skinNames[i];
                else
                    ui.skinNameText.text = $"Skin {i}";

                ui.skinImage.sprite = skinIcons.ContainsKey(i) ? skinIcons[i] : null;

                // Set button + price
                if (skin.owned)
                {
                    ui.buyButton.interactable = false;
                    ui.buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Owned";
                    ui.priceText.gameObject.SetActive(false);
                }
                else
                {
                    ui.buyButton.interactable = true;
                    ui.buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Buy";
                    ui.priceText.gameObject.SetActive(true);
                    ui.priceText.text = "Cost: " + skin.price;

                    int skinIndex = i; // capture loop variable
                    ui.buyButton.onClick.RemoveAllListeners();
                    ui.buyButton.onClick.AddListener(() => BuySkin(skinIndex));
                }
            }
        }
    }

    /// Buys a skin
    public void BuySkin(int index)
    {
        var data = PlayerDataManager.Instance?.data;

        if (data == null || data.skins == null || index < 0 || index >= data.skins.Length)
        {
            Debug.LogError($"Invalid skin index {index}");
            return;
        }

        if (!data.skins[index].owned && data.coins >= data.skins[index].price)
        {
            data.coins -= data.skins[index].price;
            data.skins[index].owned = true;

            PlayerDataManager.Instance.Save();

            RefreshShopUI();
            coinUI.UpdateCoins();

            mdm.OnInteraction();
        }
        else if (data.coins < data.skins[index].price)
        {
            mdm.OnBadInteraction();
            Debug.Log("Not enough coins to buy this skin!");
        }
    }

    private void ResizeContent(Transform scrollContent)
    {
        int childCount = scrollContent.transform.childCount;
        float elementHeight = 550f; // your prefab’s height
        float spacing = 50f;             // optional, if using a VerticalLayoutGroup

        float newHeight = (childCount * elementHeight) + (childCount - 1) * spacing + 350f;

        RectTransform rt = scrollContent.GetComponent<RectTransform>();
        if (rt != null)
        {
            Vector2 size = rt.sizeDelta;
            size.y = newHeight;
            rt.sizeDelta = size;
        }
    }
}
