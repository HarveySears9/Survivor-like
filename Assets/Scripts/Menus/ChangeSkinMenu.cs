using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChangeSkinMenu : MonoBehaviour
{
    public SceneTransitionController stc;
    public ShopTabs tabs;

    public GameObject scrollContent;

    public SkinManager sm;

    private int selectedIndex; // Stores Currently Selected Tab

    private string[] skinNames; // Stores array of Names for Skins

    public TextMeshProUGUI nameText;

    public Button equipButton;

    public GameObject[] locks;

    [Header("Sprite Database")]
    public SpriteDatabase database;

    // Start is called before the first frame update
    void Start()
    {
        skinNames = database.skinNames;

        SetUpMenu();
    }

    void SetUpMenu()
    {
        var data = PlayerDataManager.Instance?.data;
        if (data == null)
        {
            Debug.LogError("Player data not initialized!");
            return;
        }
        tabs.OnTabPressed(data.brickSkinEquipped);

        nameText.text = skinNames[data.brickSkinEquipped];

        var rect = scrollContent.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2((data.brickSkinEquipped) * -700, rect.anchoredPosition.y);

        if (equipButton != null) equipButton.interactable = false;
        if (equipButton != null) equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Equipped";


        for (int i = 0; i < locks.Length; i++)
        {
            if (!data.skins[i].owned)
            {
                locks[i].SetActive(true);
            }
        }
    }

    public void OnSkinTabPressed(int index)
    {
        var data = PlayerDataManager.Instance?.data;

        tabs.OnTabPressed(index);
        nameText.text = skinNames[index];
        if (index == data.brickSkinEquipped)
        {
            selectedIndex = index;
            if (equipButton != null) equipButton.interactable = false;
            if (equipButton != null) equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Equipped";
        }
        else if (data.skins[index].owned)
        {
            selectedIndex = index;
            if (equipButton != null) equipButton.interactable = true;
            if (equipButton != null) equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Equip";
        }
        else
        {
            selectedIndex = 0;
            if (equipButton != null) equipButton.interactable = false;
            if (equipButton != null) equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Locked";
        }
    }


    public void EquipButtonPressed()
    {
        var data = PlayerDataManager.Instance?.data;
        if (data.skins[selectedIndex].owned)
        {
            EquipSkin(selectedIndex);
            if (equipButton != null) equipButton.interactable = false;
            if (equipButton != null) equipButton.GetComponentInChildren<TextMeshProUGUI>().text = "Equipped";
        }
    }

    void EquipSkin(int index)
    {
        sm.ChangeBrickSkin(index);
    }

    public void Home()
    {
        if (Application.CanStreamedLevelBeLoaded("PuddleBrook"))
        {
            SceneTracker.UpdateLastSceneName();
            stc.TriggerTransition("PuddleBrook");
        }
        else
        {
            Debug.LogError("Scene 'Puddlebrook' not found. Please check Build Settings.");
        }
    }
}
