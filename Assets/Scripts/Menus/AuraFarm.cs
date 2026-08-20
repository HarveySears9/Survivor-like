using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AuraFarm : MonoBehaviour
{
    [Header("Aura Settings")]
    public float currentAura = 0;
    public float maxAura = 1000;
    public float auraPerSecond = 1;

    [Header("Coin Settings")]
    public float auraPerCoin = 10;

    [Header("UI")]
    public TMP_Text auraPercentageText;
    public Slider auraSlider;
    public TMP_Text coinsText;

    [Header("Aura Sprite")]
    public SpriteRenderer sr;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        // Generate Aura until the farm is full
        if (currentAura < maxAura)
        {
            currentAura += auraPerSecond * Time.deltaTime;

            // Prevent Aura going above the maximum
            currentAura = Mathf.Min(currentAura, maxAura);

            UpdateUI();
        }
    }

    public void ClaimAura()
    {
        float coinsToClaim = currentAura / auraPerCoin;

        Debug.Log("Claimed " + coinsToClaim + " coins!");

        // Later, this will add coins to your actual currency system

        currentAura = 0;

        UpdateUI();
    }

    void UpdateUI()
    {
        float percentage = (currentAura / maxAura) * 100f;

        // Update percentage text
        auraPercentageText.text = "Aura: " + Mathf.FloorToInt(percentage) + "%";

        // Update progress bar
        auraSlider.value = currentAura / maxAura;

        // Calculate coins
        float coinsToClaim = currentAura / auraPerCoin;

        coinsText.text = Mathf.FloorToInt(coinsToClaim).ToString();

        // Sprite transparency based on Aura
        float alpha = Mathf.Lerp(0f, 0.65f, currentAura / maxAura);

        Color spriteColor = sr.color;
        spriteColor.a = alpha;
        sr.color = spriteColor;
    }
}