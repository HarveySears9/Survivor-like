using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeMenu : MonoBehaviour
{
    public CoinUI coinUI;
    public TextMeshProUGUI hpUpgradeCostText;
    public TextMeshProUGUI currentHpText;
    public TextMeshProUGUI damageUpgradeCostText;
    public TextMeshProUGUI currentDamageText;
    public TextMeshProUGUI currentSpeedText;
    public TextMeshProUGUI speedUpgradeCostText;

    public Button hpUpgradeButton;
    public Button speedUpgradeButton;
    public Button damageUpgradeButton;

    public int baseHpUpgradeCost = 50;
    public int baseSpeedUpgradeCost = 50;
    public int baseDamageUpgradeCost = 50;

    public MenuDialogManager mdm;

    void OnEnable()
    {
        UpdateUI();
    }

    public void UpgradeMaxHP()
    {
        var data = PlayerDataManager.Instance.data;
        int cost = Mathf.CeilToInt(baseHpUpgradeCost * Mathf.Pow(1.5f, data.maxHPLevel));

        if (data.coins >= cost)
        {
            data.coins -= cost;
            data.maxHPLevel++;
            data.maxHP += data.maxHPLevel + 1;
            PlayerDataManager.Instance.Save();
            UpdateUI();
            mdm.OnInteraction();
        }
        else
        {
            mdm.OnBadInteraction();
        }
    }

    public void UpgradeSpeed()
    {
        var data = PlayerDataManager.Instance.data;
        int cost = Mathf.CeilToInt(baseSpeedUpgradeCost * Mathf.Pow(1.25f, data.speedLevel));

        if (data.coins >= cost)
        {
            data.coins -= cost;
            data.speedLevel++;
            PlayerDataManager.Instance.Save();
            UpdateUI();
            mdm.OnInteraction();
        }
        else
        {
            mdm.OnBadInteraction();
        }
    }

    public void UpgradeDamage()
    {
        var data = PlayerDataManager.Instance.data;
        int cost = Mathf.CeilToInt(baseDamageUpgradeCost * Mathf.Pow(1.5f, data.currentDamage - 1));

        if (data.coins >= cost)
        {
            data.coins -= cost;
            data.currentDamage++;
            PlayerDataManager.Instance.Save();
            UpdateUI();
            mdm.OnInteraction();
        }
        else
        {
            mdm.OnBadInteraction();
        }
    }

    private void UpdateUI()
    {
        var data = PlayerDataManager.Instance.data;

        currentHpText.text = $"Max HP: {data.maxHP}";
        currentDamageText.text = $"Damage: {data.currentDamage}";
        currentSpeedText.text = $"Speed: {100f * (1f + data.speedLevel * 0.05f)}%";
        coinUI.UpdateCoins();

        hpUpgradeCostText.text = $"Cost: {Mathf.CeilToInt(baseHpUpgradeCost * Mathf.Pow(1.5f, data.maxHPLevel))}";
        speedUpgradeCostText.text = $"Cost: {Mathf.CeilToInt(baseSpeedUpgradeCost * Mathf.Pow(1.25f, data.speedLevel))}";
        damageUpgradeCostText.text = $"Cost: {Mathf.CeilToInt(baseDamageUpgradeCost * Mathf.Pow(1.5f, data.currentDamage - 1))}";

        //hpUpgradeButton.interactable = data.coins >= Mathf.CeilToInt(baseHpUpgradeCost * Mathf.Pow(1.5f, data.maxHPLevel));
        //speedUpgradeButton.interactable = data.coins >= Mathf.CeilToInt(baseSpeedUpgradeCost * Mathf.Pow(1.25f, data.speedLevel));
        //damageUpgradeButton.interactable = data.coins >= Mathf.CeilToInt(baseDamageUpgradeCost * Mathf.Pow(1.5f, data.currentDamage - 1));
    }
}
