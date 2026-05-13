using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeMenu : MonoBehaviour
{
    [Header("Data")]
    public UpgradeDefinition hpUpgrade;
    public UpgradeDefinition damageUpgrade;
    public UpgradeDefinition speedUpgrade;
    public UpgradeDefinition pickupUpgrade;

    [Header("UI")]
    public CoinUI coinUI;
    public MenuDialogManager mdm;

    public TextMeshProUGUI hpText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI pickupText;

    public TextMeshProUGUI hpCostText;
    public TextMeshProUGUI damageCostText;
    public TextMeshProUGUI speedCostText;
    public TextMeshProUGUI pickupCostText;

    private SaveFile.Data data => PlayerDataManager.Instance.data;

    void OnEnable()
    {
        UpdateUI();
    }

    // =========================
    // GENERIC UPGRADE FUNCTION
    // =========================
    private void TryUpgrade(UpgradeDefinition upgrade, ref int level)
    {
        if (upgrade.IsMax(level))
            return;

        int cost = upgrade.GetCost(level);

        if (data.coins >= cost)
        {
            data.coins -= cost;
            level++;

            PlayerDataManager.Instance.Save();
            UpdateUI();
            mdm.OnInteraction();
        }
        else
        {
            mdm.OnBadInteraction();
        }
    }

    // =========================
    // BUTTONS
    // =========================
    public void UpgradeHP() => TryUpgrade(hpUpgrade, ref data.maxHPLevel);
    public void UpgradeDamage() => TryUpgrade(damageUpgrade, ref data.damageLevel);
    public void UpgradeSpeed() => TryUpgrade(speedUpgrade, ref data.speedLevel);
    public void UpgradePickup() => TryUpgrade(pickupUpgrade, ref data.pickupRadiusLevel);

    // =========================
    // UI
    // =========================
    private void UpdateUI()
    {
        coinUI.UpdateCoins();

        UpdateUpgradeUI(hpUpgrade, data.maxHPLevel, hpText, hpCostText, PlayerStats.GetMaxHP().ToString());
        UpdateUpgradeUI(damageUpgrade, data.damageLevel, damageText, damageCostText, PlayerStats.GetDamageMultiplier().ToString("0.00") + "x");
        UpdateUpgradeUI(speedUpgrade, data.speedLevel, speedText, speedCostText, PlayerStats.GetSpeedMultiplier().ToString("0%"));
        UpdateUpgradeUI(pickupUpgrade, data.pickupRadiusLevel, pickupText, pickupCostText, PlayerStats.GetPickupRadius().ToString("0.00"));
    }

    private void UpdateUpgradeUI(
        UpgradeDefinition upgrade,
        int level,
        TextMeshProUGUI valueText,
        TextMeshProUGUI costText,
        string valueDisplay)
    {
        valueText.text = valueDisplay;

        if (upgrade.IsMax(level))
        {
            costText.text = "MAX";
        }
        else
        {
            costText.text = $"Cost: {upgrade.GetCost(level)}";
        }
    }
}