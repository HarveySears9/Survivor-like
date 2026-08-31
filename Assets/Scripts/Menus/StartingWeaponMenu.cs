using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartingWeaponMenu : MonoBehaviour
{
    public ShopTabs tabs;

    public Button equipButton;

    public TextMeshProUGUI nameText;

    public string[] weaponNames;

    public GameObject[] locks;

    private int selectedIndex;

    public SceneTransitionController stc;

    public GameObject scrollContent;

    public int[] weaponOrder;

    private void Start()
    {
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

        int equippedIndex = data.startingWeapon;

        selectedIndex = equippedIndex;

        // Select the currently equipped tab.
        tabs.OnTabPressed(equippedIndex);

        // Set name.
        nameText.text = weaponNames[equippedIndex];

        // Currently equipped weapon.
        SetEquipButton(false, "Equipped");

        int uiIndex = System.Array.IndexOf(weaponOrder, equippedIndex);

        var rect = scrollContent.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(uiIndex * -700, rect.anchoredPosition.y);

        // Set lock visuals.
        for (int i = 0; i < locks.Length; i++)
        {
            if (i < data.weaponUnlocks.Length &&
                i < data.startingWeaponUnlocks.Length)
            {
                bool unlocked =
                    data.weaponUnlocks[i] &&
                    data.startingWeaponUnlocks[i];

                locks[i].SetActive(!unlocked);
            }
            else
            {
                // If the arrays don't contain this weapon,
                // keep it locked.
                locks[i].SetActive(true);
            }
        }
    }


    public void OnWeaponTabPressed(int index)
    {
        var data = PlayerDataManager.Instance?.data;

        if (data == null)
        {
            Debug.LogError("Player data not initialized!");
            return;
        }

        tabs.OnTabPressed(index);

        nameText.text = weaponNames[index];


        // Already equipped.
        if (index == data.startingWeapon)
        {
            selectedIndex = index;

            SetEquipButton(false, "Equipped");

            return;
        }


        // Weapon must be unlocked normally AND unlocked
        // as a starting weapon.
        bool canStartWithWeapon =
            index < data.weaponUnlocks.Length &&
            index < data.startingWeaponUnlocks.Length &&
            data.weaponUnlocks[index] &&
            data.startingWeaponUnlocks[index];


        if (canStartWithWeapon)
        {
            selectedIndex = index;

            SetEquipButton(true, "Equip");
        }
        else
        {
            // Weapon is locked.
            selectedIndex = data.startingWeapon;

            SetEquipButton(false, "Locked");
        }
    }


    public void EquipButtonPressed()
    {
        var data = PlayerDataManager.Instance?.data;

        if (data == null)
        {
            Debug.LogError("Player data not initialized!");
            return;
        }


        // Make sure the selected weapon exists in both arrays.
        if (selectedIndex >= data.weaponUnlocks.Length ||
            selectedIndex >= data.startingWeaponUnlocks.Length)
        {
            Debug.LogError("Invalid starting weapon index: " + selectedIndex);
            return;
        }


        // Weapon must be unlocked normally AND
        // unlocked as a starting weapon.
        if (!data.weaponUnlocks[selectedIndex] ||
            !data.startingWeaponUnlocks[selectedIndex])
        {
            Debug.LogWarning(
                "Cannot equip weapon as starting weapon. " +
                "Weapon is not fully unlocked."
            );

            return;
        }


        // Equip the starting weapon.
        data.startingWeapon = selectedIndex;

        // Save the data.
        PlayerDataManager.Instance.Save();


        // Update button.
        SetEquipButton(false, "Equipped");
    }


    private void SetEquipButton(bool interactable, string text)
    {
        if (equipButton == null)
            return;

        equipButton.interactable = interactable;

        TextMeshProUGUI buttonText =
            equipButton.GetComponentInChildren<TextMeshProUGUI>();

        if (buttonText != null)
            buttonText.text = text;
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