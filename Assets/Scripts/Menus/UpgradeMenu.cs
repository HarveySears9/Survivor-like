using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeMenu : MonoBehaviour
{
    public TextMeshProUGUI coinText;          // Displays total coins
    public TextMeshProUGUI upgradeCostText;   // Displays cost of next Max HP upgrade
    public TextMeshProUGUI currentHpText;     // Displays current Max HP
    public TextMeshProUGUI currentSpeedText;  // Displays current movement speed
    public TextMeshProUGUI speedUpgradeCostText; // Displays cost of the next Speed upgrade

    public Button hpUpgradeButton;           // Button to upgrade Max HP
    public Button speedUpgradeButton;        // Button to upgrade movement speed

    public int baseHpUpgradeCost = 50;       // Cost of the first Max HP upgrade
    public int baseSpeedUpgradeCost = 50;    // Cost of the first speed upgrade

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
        int upgradeCost = baseHpUpgradeCost * (int)Mathf.Pow(1.5f, playerData.maxHPLevel); // Cost doubles with each level

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
        int upgradeCost = baseSpeedUpgradeCost * (int)Mathf.Pow(1.25f, playerData.speedLevel); // Cost doubles with each level

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

    private void UpdateUI()
    {
        // Update Max HP display
        currentHpText.text = $"Max HP: {playerData.maxHP}";

        // Update Speed display
        float baseSpeed = 2f; // Default player speed
        float speedMultiplier = 1f + playerData.speedLevel * 0.05f; // 5% increase per level
        currentSpeedText.text = $"Speed: {(100f*(1f + playerData.speedLevel * 0.05f)).ToString()}%";

        // Update coin text
        coinText.text = $"Coins: {playerData.coins}";

        // Update upgrade costs
        int nextHpUpgradeCost = baseHpUpgradeCost * (int)Mathf.Pow(1.5f, playerData.maxHPLevel);
        upgradeCostText.text = $"Cost: {nextHpUpgradeCost}";

        int nextSpeedUpgradeCost = baseSpeedUpgradeCost * (int)Mathf.Pow(1.25f, playerData.speedLevel);
        speedUpgradeCostText.text = $"Cost: {nextSpeedUpgradeCost}";

        // Enable/disable buttons based on coins
        hpUpgradeButton.interactable = playerData.coins >= nextHpUpgradeCost;
        speedUpgradeButton.interactable = playerData.coins >= nextSpeedUpgradeCost;
    }
}
