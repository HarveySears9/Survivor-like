using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeMenu : MonoBehaviour
{
    public CoinUI coinUI;
    public MenuDialogManager mdm;

    [Header("UI Text")]
    public TextMeshProUGUI hpUpgradeCostText;
    public TextMeshProUGUI currentHpText;

    public TextMeshProUGUI damageUpgradeCostText;
    public TextMeshProUGUI currentDamageText;

    public TextMeshProUGUI speedUpgradeCostText;
    public TextMeshProUGUI currentSpeedText;

    public TextMeshProUGUI pickupUpgradeCostText;
    public TextMeshProUGUI currentPickupText;

    [Header("Buttons")]
    public Button hpUpgradeButton;
    public Button speedUpgradeButton;
    public Button damageUpgradeButton;
    public Button pickupUpgradeButton;

    [Header("Base Costs")]
    public int baseHpUpgradeCost = 50;
    public int baseSpeedUpgradeCost = 50;
    public int baseDamageUpgradeCost = 50;
    public int basePickupUpgradeCost = 50;

    private SaveFile.Data data => PlayerDataManager.Instance.data;

    void OnEnable()
    {
        UpdateUI();
    }

    // =========================
    // HP
    // =========================
    public void UpgradeHP()
    {
        int cost = Mathf.CeilToInt(baseHpUpgradeCost * Mathf.Pow(1.5f, data.maxHPLevel));

        if (data.coins >= cost)
        {
            data.coins -= cost;
            data.maxHPLevel++;

            PlayerDataManager.Instance.Save();
            UpdateUI();
            mdm.OnInteraction();
        }
        else mdm.OnBadInteraction();
    }

    // =========================
    // DAMAGE
    // =========================
    public void UpgradeDamage()
    {
        int cost = Mathf.CeilToInt(baseDamageUpgradeCost * Mathf.Pow(1.5f, data.damageLevel));

        if (data.coins >= cost)
        {
            data.coins -= cost;
            data.damageLevel++;

            PlayerDataManager.Instance.Save();
            UpdateUI();
            mdm.OnInteraction();
        }
        else mdm.OnBadInteraction();
    }

    // =========================
    // SPEED
    // =========================
    public void UpgradeSpeed()
    {
        int cost = Mathf.CeilToInt(baseSpeedUpgradeCost * Mathf.Pow(1.25f, data.speedLevel));

        if (data.coins >= cost)
        {
            data.coins -= cost;
            data.speedLevel++;

            PlayerDataManager.Instance.Save();
            UpdateUI();
            mdm.OnInteraction();
        }
        else mdm.OnBadInteraction();
    }

    // =========================
    // PICKUP RADIUS
    // =========================
    public void UpgradePickup()
    {
        int cost = Mathf.CeilToInt(basePickupUpgradeCost * Mathf.Pow(1.4f, data.pickupRadiusLevel));

        if (data.coins >= cost)
        {
            data.coins -= cost;
            data.pickupRadiusLevel++;

            PlayerDataManager.Instance.Save();
            UpdateUI();
            mdm.OnInteraction();
        }
        else mdm.OnBadInteraction();
    }

    // =========================
    // UI
    // =========================
    private void UpdateUI()
    {
        coinUI.UpdateCoins();

        // HP
        currentHpText.text =
            $"Max HP: {PlayerStats.GetMaxHP()}";

        hpUpgradeCostText.text =
            $"Cost: {Mathf.CeilToInt(baseHpUpgradeCost * Mathf.Pow(1.5f, data.maxHPLevel))}";

        // DAMAGE
        currentDamageText.text =
            $"Damage: {PlayerStats.GetDamageMultiplier():0.00}x";

        damageUpgradeCostText.text =
            $"Cost: {Mathf.CeilToInt(baseDamageUpgradeCost * Mathf.Pow(1.5f, data.damageLevel))}";

        // SPEED
        currentSpeedText.text =
            $"Speed: {PlayerStats.GetSpeedMultiplier():0%}";

        speedUpgradeCostText.text =
            $"Cost: {Mathf.CeilToInt(baseSpeedUpgradeCost * Mathf.Pow(1.25f, data.speedLevel))}";

        // PICKUP RADIUS
        currentPickupText.text =
            $"Pickup Radius: {PlayerStats.GetPickupRadius():0.00}";

        pickupUpgradeCostText.text =
            $"Cost: {Mathf.CeilToInt(basePickupUpgradeCost * Mathf.Pow(1.4f, data.pickupRadiusLevel))}";
    }
}