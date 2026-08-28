using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuraFarm : MonoBehaviour
{
    [Header("Aura Settings")]
    public float currentAura = 0f;
    public float maxAura = 1000f;
    public float auraPerSecond = 1f;

    [Header("Coin Settings")]
    public float auraPerCoin = 10f;

    [Header("UI")]
    public TMP_Text auraPercentageText;
    public Slider auraSlider;
    public TMP_Text coinsText;
    public CoinUI coinUI;

    [Header("Aura Sprite")]
    public SpriteRenderer sr;

    private DateTime lastAuraUpdateTime;

    void Start()
    {
        LoadAuraFarm();
        UpdateAura();
        UpdateUI();
        coinUI.UpdateCoins();
    }


    void Update()
    {
        UpdateAura();
    }

    void UpdateAura()
    {
        DateTime currentTime = DateTime.UtcNow;

        // how many seconds have passed
        double elapsedSeconds =
            (currentTime - lastAuraUpdateTime).TotalSeconds;

        if (elapsedSeconds <= 0)
            return;


        // How much Aura can still fit in the farm?
        float remainingAura = maxAura - currentAura;

        if (remainingAura <= 0)
        {
            // Farm is already full.
            // Keep the timestamp where it is so we don't
            // accidentally generate Aura beyond the capacity.
            UpdateUI();
            return;
        }


        // Calculate how much Aura should have been generated
        float auraGenerated =
            (float)elapsedSeconds * auraPerSecond;


        // If we would go over the maximum,
        // only add enough to fill the farm.
        if (auraGenerated >= remainingAura)
        {
            currentAura = maxAura;

            // Work out exactly how long it took to fill the farm
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

    void LoadAuraFarm()
    {
        long savedTicks =
            PlayerDataManager.Instance.data.auraLastUpdate;


        // No previous Aura timestamp exists.
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

    void SaveAuraTimestamp()
    {
        PlayerDataManager.Instance.data.auraLastUpdate =
            lastAuraUpdateTime.Ticks;

        PlayerDataManager.Instance.Save();
    }


    public void ClaimAura()
    {
        // Make absolutely sure we have the latest Aura amount
        UpdateAura();


        // Convert Aura into coins
        int coinsToClaim =
            Mathf.FloorToInt(currentAura / auraPerCoin);


        // Don't do anything if there aren't enough Aura
        if (coinsToClaim <= 0)
        {
            Debug.Log("Not enough Aura to claim.");
            return;
        }


        Debug.Log("Claimed " + coinsToClaim + " coins!");


        // Add coins to the player's save data
        PlayerDataManager.Instance.data.coins += coinsToClaim;


        // Empty the Aura farm
        currentAura = 0f;


        // Start generating from this exact moment
        lastAuraUpdateTime = DateTime.UtcNow;


        // Save the new timestamp and coin total
        SaveAuraTimestamp();


        // Update UI
        UpdateUI();

        coinUI.UpdateCoins();
    }

    void UpdateUI()
    {
        // Prevent division by zero
        if (maxAura <= 0)
            return;


        // Aura percentage as a value between 0 and 1
        float percentage =
            Mathf.Clamp01(currentAura / maxAura);


        // Percentage text
        auraPercentageText.text =
            "Aura: " +
            Mathf.FloorToInt(percentage * 100f) +
            "%";


        // Progress bar
        auraSlider.value = percentage;


        // Coins that can currently be claimed
        int coinsToClaim =
            Mathf.FloorToInt(currentAura / auraPerCoin);


        coinsText.text =
            coinsToClaim.ToString();


        // Sprite alpha
        // 0% Aura = 0 alpha
        // 100% Aura = 0.65 alpha
        float alpha =
            Mathf.Lerp(0f, 0.65f, percentage);


        Color spriteColor = sr.color;

        spriteColor.a = alpha;

        sr.color = spriteColor;
    }
}