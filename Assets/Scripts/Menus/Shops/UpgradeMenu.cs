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

    [Header("Buttons")]
    public Button hpButton;
    public Button damageButton;
    public Button speedButton;
    public Button pickupButton;

    [Header("Button Text")]
    public TextMeshProUGUI hpButtonText;
    public TextMeshProUGUI damageButtonText;
    public TextMeshProUGUI speedButtonText;
    public TextMeshProUGUI pickupButtonText;

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

        UpdateButtonState(hpUpgrade, data.maxHPLevel, hpButton, hpButtonText);
        UpdateButtonState(damageUpgrade, data.damageLevel, damageButton, damageButtonText);
        UpdateButtonState(speedUpgrade, data.speedLevel, speedButton, speedButtonText);
        UpdateButtonState(pickupUpgrade, data.pickupRadiusLevel, pickupButton, pickupButtonText);
    }

    private void UpdateUpgradeUI(
        UpgradeDefinition upgrade,
        int level,
        TextMeshProUGUI valueText,
        TextMeshProUGUI costText,
        string valueDisplay)
    {
        valueText.text = $"{upgrade.displayName}: {valueDisplay}";

        if (upgrade.IsMax(level))
        {
            costText.text = "MAX";
        }
        else
        {
            costText.text = $"Cost: {upgrade.GetCost(level)}";
        }
    }

    private void UpdateButtonState(
    UpgradeDefinition upgrade,
    int level,
    Button button,
    TextMeshProUGUI buttonText)
    {
        if (upgrade.IsMax(level))
        {
            button.interactable = false;
            buttonText.text = "MAX";
        }
        else
        {
            button.interactable = true;
            buttonText.text = "UPGRADE";
        }
    }
}