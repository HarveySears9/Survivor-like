using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeMenu : MonoBehaviour
{
    public TextMeshProUGUI coinText;          // Displays total coins
    public TextMeshProUGUI hpUpgradeCostText;   // Displays cost of next Max HP upgrade
    public TextMeshProUGUI currentHpText;     // Displays current Max HP

    public TextMeshProUGUI damageUpgradeCostText;
    public TextMeshProUGUI currentDamageText;

    public TextMeshProUGUI currentSpeedText;  // Displays current movement speed
    public TextMeshProUGUI speedUpgradeCostText; // Displays cost of the next Speed upgrade

    public Button hpUpgradeButton;           // Button to upgrade Max HP
    public Button speedUpgradeButton;        // Button to upgrade movement speed
    public Button damageUpgradeButton;

    public int baseHpUpgradeCost = 50;       // Cost of the first Max HP upgrade
    public int baseSpeedUpgradeCost = 50;    // Cost of the first speed upgrade
    public int baseDamageUpgradeCost = 50;

    private SaveFile.Data playerData;

    void Start()
    {
        // Load saved data
        playerData = SaveFile.LoadData<SaveFile.Data>();

        UpdateUI();
    }

    public void UpgradeMaxHP()
    {
        // Calculate upgrade cost
        int upgradeCost = Mathf.CeilToInt(baseHpUpgradeCost * Mathf.Pow(1.5f, playerData.maxHPLevel)); 

        if (playerData.coins >= upgradeCost)
        {
            // Deduct coins and increase Max HP level
            playerData.coins -= upgradeCost;
            playerData.maxHPLevel++;

            playerData.maxHP += playerData.maxHPLevel+1;

            // Save the updated data
            SaveFile.SaveData(playerData);

            // Update UI
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    public void UpgradeSpeed()
    {
        // Calculate upgrade cost
        int upgradeCost = Mathf.CeilToInt(baseSpeedUpgradeCost * Mathf.Pow(1.25f, playerData.speedLevel)); // Cost doubles with each level

        if (playerData.coins >= upgradeCost)
        {
            // Deduct coins and increase Speed level
            playerData.coins -= upgradeCost;
            playerData.speedLevel++;

            // Save the updated data
            SaveFile.SaveData(playerData);

            // Update UI
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    public void UpgradeDamage()
    {
        // Calculate upgrade cost
        int upgradeCost = Mathf.CeilToInt(baseDamageUpgradeCost * Mathf.Pow(1.5f, playerData.currentDamage - 1)); // Cost doubles with each level

        if (playerData.coins >= upgradeCost)
        {
            // Deduct coins and increase Max HP level
            playerData.coins -= upgradeCost;
            playerData.currentDamage++;

            // Save the updated data
            SaveFile.SaveData(playerData);

            // Update UI
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    private void UpdateUI()
    {
        // Update Max HP display
        currentHpText.text = $"Max HP: {playerData.maxHP}";

        currentDamageText.text = "Damage:"+playerData.currentDamage.ToString();

        // Update Speed display
        float speedMultiplier = 1f + playerData.speedLevel * 0.05f; // 5% increase per level
        currentSpeedText.text = $"Speed: {(100f*(1f + playerData.speedLevel * 0.05f)).ToString()}%";

        // Update coin text
        coinText.text = $"Coins: {playerData.coins}";

        // Update upgrade costs
        // For HP upgrade cost (exponential growth)
        int nextHpUpgradeCost = Mathf.CeilToInt(baseHpUpgradeCost * Mathf.Pow(1.5f, playerData.maxHPLevel));
        hpUpgradeCostText.text = $"Cost: {nextHpUpgradeCost}";

        // For Speed upgrade cost (exponential growth)
        int nextSpeedUpgradeCost = Mathf.CeilToInt(baseSpeedUpgradeCost * Mathf.Pow(1.25f, playerData.speedLevel));
        speedUpgradeCostText.text = $"Cost: {nextSpeedUpgradeCost}";

        // For Damage upgrade cost (exponential growth)
        int nextDamageUpgradeCost = Mathf.CeilToInt(baseDamageUpgradeCost * Mathf.Pow(1.5f, playerData.currentDamage - 1));
        damageUpgradeCostText.text = $"Cost: {nextDamageUpgradeCost}";


        // Enable/disable buttons based on coins
        hpUpgradeButton.interactable = playerData.coins >= nextHpUpgradeCost;
        speedUpgradeButton.interactable = playerData.coins >= nextSpeedUpgradeCost;
        damageUpgradeButton.interactable = playerData.coins >= nextDamageUpgradeCost;
    }
}
