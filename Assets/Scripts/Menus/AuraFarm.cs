using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuraFarm : MonoBehaviour
{
    [Header("Aura Settings")]
    public float currentAura = 0f;

    // The amount of Aura stored at each level
    private readonly float[] auraCapacityByLevel =
    {
        1000f,  // Level 1
        1500f,  // Level 2
        2250f   // Level 3
    };

    private readonly int[] upgradeCosts =
    {
        500,    // Level 1 -> Level 2
        1500    // Level 2 -> Level 3
    };

    // The farm will always take this many hours to become full
    public float fillTimeHours = 12f;

    [Header("Coin Settings")]
    public float auraPerCoin = 10f;

    [Header("UI")]
    public TMP_Text auraPercentageText;
    public Slider auraSlider;
    public TMP_Text coinsText;
    public CoinUI coinUI;

    [Header("Aura Sprite")]
    public SpriteRenderer sr;

    private float maxAura;
    private float auraPerSecond;

    private DateTime lastAuraUpdateTime;

    public MenuDialogManager dialogue;


    // ============================================================
    // START
    // ============================================================

    void Start()
    {
        SetupAuraLevel();

        LoadAuraFarm();

        UpdateAura();

        UpdateUI();

        coinUI.UpdateCoins();
    }


    // ============================================================
    // SETUP LEVEL
    // ============================================================

    void SetupAuraLevel()
    {
        int level = PlayerDataManager.Instance.data.auraLevel;

        // Make sure the level is within our available levels
        level = Mathf.Clamp(level, 1, auraCapacityByLevel.Length);

        // Array starts at 0, so Level 1 = index 0
        maxAura = auraCapacityByLevel[level - 1];

        // Calculate Aura per second so the farm
        // always takes exactly 12 hours to fill
        auraPerSecond =
            maxAura / (fillTimeHours * 60f * 60f);
    }


    // ============================================================
    // UPDATE
    // ============================================================

    void Update()
    {
        UpdateAura();
    }


    // ============================================================
    // AURA GENERATION
    // ============================================================

    void UpdateAura()
    {
        DateTime currentTime = DateTime.UtcNow;

        double elapsedSeconds =
            (currentTime - lastAuraUpdateTime).TotalSeconds;

        if (elapsedSeconds <= 0)
            return;


        // How much Aura can still fit?
        float remainingAura = maxAura - currentAura;


        // Farm is already full
        if (remainingAura <= 0)
        {
            UpdateUI();
            return;
        }


        // Calculate how much Aura should have been generated
        float auraGenerated =
            (float)elapsedSeconds * auraPerSecond;


        // If the generated Aura would exceed the capacity,
        // fill the farm exactly.
        if (auraGenerated >= remainingAura)
        {
            currentAura = maxAura;

            // Work out exactly when the farm became full
            double secondsNeeded =
                remainingAura / auraPerSecond;

            lastAuraUpdateTime =
                lastAuraUpdateTime.AddSeconds(secondsNeeded);
        }
        else
        {
            currentAura += auraGenerated;

            lastAuraUpdateTime = currentTime;
        }


        UpdateUI();
    }


    // ============================================================
    // LOAD
    // ============================================================

    void LoadAuraFarm()
    {
        long savedTicks =
            PlayerDataManager.Instance.data.auraLastUpdate;


        // No timestamp exists
        if (savedTicks <= 0)
        {
            lastAuraUpdateTime = DateTime.UtcNow;

            SaveAuraTimestamp();
        }
        else
        {
            lastAuraUpdateTime =
                new DateTime(savedTicks, DateTimeKind.Utc);
        }
    }


    // ============================================================
    // SAVE TIMESTAMP
    // ============================================================

    void SaveAuraTimestamp()
    {
        PlayerDataManager.Instance.data.auraLastUpdate =
            lastAuraUpdateTime.Ticks;

        PlayerDataManager.Instance.Save();
    }


    // ============================================================
    // CLAIM AURA
    // ============================================================

    public void ClaimAura()
    {
        // Make sure Aura is completely up-to-date
        UpdateAura();


        // Convert Aura into coins
        int coinsToClaim =
            Mathf.FloorToInt(currentAura / auraPerCoin);


        if (coinsToClaim <= 0)
        {
            dialogue.OnBadInteraction();
            Debug.Log("Not enough Aura to claim.");
            return;
        }


        Debug.Log(
            "Claimed " + coinsToClaim + " coins!"
        );


        // Add coins to player's save data
        PlayerDataManager.Instance.data.coins +=
            coinsToClaim;


        // Empty the farm
        currentAura = 0f;


        // Start a new 12-hour cycle
        lastAuraUpdateTime =
            DateTime.UtcNow;


        // Save
        SaveAuraTimestamp();


        // Update UI
        UpdateUI();

        coinUI.UpdateCoins();

        dialogue.OnInteraction();
    }


    // ============================================================
    // UI
    // ============================================================

    void UpdateUI()
    {
        if (maxAura <= 0)
            return;


        // Percentage between 0 and 1
        float percentage =
            Mathf.Clamp01(currentAura / maxAura);


        // Percentage text
        auraPercentageText.text =
            "Aura: " +
            Mathf.FloorToInt(percentage * 100f) +
            "%";


        // Progress bar
        auraSlider.value = percentage;


        // Coins available
        int coinsToClaim =
            Mathf.FloorToInt(currentAura / auraPerCoin);


        coinsText.text = coinsToClaim.ToString();


        // Aura sprite alpha
        // 0% = invisible
        // 100% = 65% opacity
        float alpha =
            Mathf.Lerp(0f, 0.65f, percentage);


        Color spriteColor = sr.color;

        spriteColor.a = alpha;

        sr.color = spriteColor;
    }

    public void UpgradeAuraFarm()
    {
        // Get current level
        int currentLevel =
            PlayerDataManager.Instance.data.auraLevel;


        // Check if the farm is already max level
        if (currentLevel >= auraCapacityByLevel.Length)
        {
            Debug.Log("Aura Farm is already max level.");

            dialogue.OnBadInteraction();

            return;
        }


        // Work out the upgrade cost
        int upgradeCost =
            upgradeCosts[currentLevel - 1];


        // Check if player can afford it
        if (PlayerDataManager.Instance.data.coins < upgradeCost)
        {
            Debug.Log(
                "Not enough coins to upgrade Aura Farm. " +
                "Cost: " + upgradeCost
            );

            dialogue.OnBadInteraction();

            return;
        }


        // --------------------------------------------------------
        // Update Aura BEFORE changing the level
        // --------------------------------------------------------
        //
        // This makes sure the player gets all Aura they
        // earned up until the exact moment of upgrading.
        //
        UpdateAura();


        // Remove the upgrade cost
        PlayerDataManager.Instance.data.coins -=
            upgradeCost;


        // Increase Aura Farm level
        currentLevel++;

        PlayerDataManager.Instance.data.auraLevel =
            currentLevel;


        // --------------------------------------------------------
        // Recalculate the farm stats
        // --------------------------------------------------------

        SetupAuraLevel();


        // --------------------------------------------------------
        // Save
        // --------------------------------------------------------

        PlayerDataManager.Instance.Save();


        // --------------------------------------------------------
        // Update UI
        // --------------------------------------------------------

        UpdateUI();

        coinUI.UpdateCoins();


        Debug.Log(
            "Aura Farm upgraded to Level " +
            currentLevel
        );


        // Successful upgrade dialogue
        dialogue.OnInteraction();
    }
}