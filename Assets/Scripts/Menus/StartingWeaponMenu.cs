using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartingWeaponMenu : MonoBehaviour
{
    public CoinUI coinUI;

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

        coinUI.UpdateCoins();

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


        // Move scroll content to equipped weapon.
        int uiIndex = System.Array.IndexOf(weaponOrder, equippedIndex);

        if (uiIndex >= 0)
        {
            var rect = scrollContent.GetComponent<RectTransform>();

            rect.anchoredPosition = new Vector2(
                uiIndex * -700,
                rect.anchoredPosition.y
            );
        }


        // Set lock visuals.
        for (int i = 0; i < locks.Length; i++)
        {
            if (i < data.weaponUnlocks.Length &&
                i < data.startingWeaponUnlocks.Length)
            {
                bool weaponUnlocked = data.weaponUnlocks[i];
                bool startingWeaponUnlocked = data.startingWeaponUnlocks[i];

                // Lock stays visible unless BOTH are unlocked.
                bool fullyUnlocked =
                    weaponUnlocked &&
                    startingWeaponUnlocked;

                locks[i].SetActive(!fullyUnlocked);
            }
            else
            {
                // If the weapon doesn't exist in the save arrays,
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


        // Make sure the index is valid.
        if (index < 0 ||
            index >= weaponNames.Length ||
            index >= data.weaponUnlocks.Length ||
            index >= data.startingWeaponUnlocks.Length ||
            index >= data.startingWeaponCosts.Length)
        {
            Debug.LogError("Invalid weapon index: " + index);
            return;
        }


        tabs.OnTabPressed(index);

        nameText.text = weaponNames[index];


        // ---------------------------------------------------------
        // ALREADY EQUIPPED
        // ---------------------------------------------------------

        if (index == data.startingWeapon)
        {
            selectedIndex = index;

            SetEquipButton(false, "Equipped");

            return;
        }


        bool weaponUnlocked = data.weaponUnlocks[index];

        bool startingWeaponUnlocked =
            data.startingWeaponUnlocks[index];


        // ---------------------------------------------------------
        // WEAPON NOT UNLOCKED
        // ---------------------------------------------------------

        if (!weaponUnlocked)
        {
            selectedIndex = index;

            SetEquipButton(false, "Locked");

            return;
        }


        // ---------------------------------------------------------
        // STARTING WEAPON ALREADY UNLOCKED
        // ---------------------------------------------------------

        if (startingWeaponUnlocked)
        {
            selectedIndex = index;

            SetEquipButton(true, "Equip");

            return;
        }


        // ---------------------------------------------------------
        // WEAPON UNLOCKED BUT NOT UNLOCKED AS STARTING WEAPON
        // ---------------------------------------------------------

        selectedIndex = index;

        int cost = data.startingWeaponCosts[index];

        SetEquipButton(
            data.coins >= cost,
            cost.ToString()
        );
    }


    public void EquipButtonPressed()
    {
        var data = PlayerDataManager.Instance?.data;

        if (data == null)
        {
            Debug.LogError("Player data not initialized!");
            return;
        }


        // Make sure the selected weapon exists in all arrays.
        if (selectedIndex < 0 ||
            selectedIndex >= data.weaponUnlocks.Length ||
            selectedIndex >= data.startingWeaponUnlocks.Length ||
            selectedIndex >= data.startingWeaponCosts.Length)
        {
            Debug.LogError(
                "Invalid starting weapon index: " +
                selectedIndex
            );

            return;
        }


        bool weaponUnlocked =
            data.weaponUnlocks[selectedIndex];

        bool startingWeaponUnlocked =
            data.startingWeaponUnlocks[selectedIndex];


        // ---------------------------------------------------------
        // WEAPON NOT UNLOCKED
        // ---------------------------------------------------------

        if (!weaponUnlocked)
        {
            Debug.LogWarning(
                "Cannot unlock this weapon as a starting weapon. " +
                "The weapon itself is still locked."
            );

            return;
        }


        // ---------------------------------------------------------
        // STARTING WEAPON ALREADY UNLOCKED
        // ---------------------------------------------------------

        if (startingWeaponUnlocked)
        {
            // Equip it.
            data.startingWeapon = selectedIndex;

            PlayerDataManager.Instance.Save();

            SetEquipButton(false, "Equipped");

            return;
        }


        // ---------------------------------------------------------
        // BUY STARTING WEAPON UNLOCK
        // ---------------------------------------------------------

        int cost =
            data.startingWeaponCosts[selectedIndex];


        // Make sure the player can afford it.
        if (data.coins < cost)
        {
            Debug.LogWarning("Not enough coins.");

            return;
        }


        // Remove coins.
        data.coins -= cost;


        // Unlock this weapon as a starting weapon.
        data.startingWeaponUnlocks[selectedIndex] = true;


        // Equip it immediately.
        data.startingWeapon = selectedIndex;


        // Remove the lock icon.
        if (selectedIndex < locks.Length)
        {
            locks[selectedIndex].SetActive(false);
        }


        // Save everything.
        PlayerDataManager.Instance.Save();

        coinUI.UpdateCoins();

        // Update button.
        SetEquipButton(false, "Equipped");


        Debug.Log(
            weaponNames[selectedIndex] +
            " unlocked as a starting weapon for " +
            cost +
            " coins."
        );
    }


    private void SetEquipButton(bool interactable, string text)
    {
        if (equipButton == null)
            return;


        equipButton.interactable = interactable;


        TextMeshProUGUI buttonText =
            equipButton.GetComponentInChildren<TextMeshProUGUI>();


        if (buttonText != null)
        {
            buttonText.text = text;
        }
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
            Debug.LogError(
                "Scene 'Puddlebrook' not found. " +
                "Please check Build Settings."
            );
        }
    }
}