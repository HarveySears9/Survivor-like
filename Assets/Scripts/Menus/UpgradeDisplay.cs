using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDisplay : MonoBehaviour
{
    public GameObject[] upgradeButtons; // Array of all upgrade button GameObjects

    void OnEnable()
    {
        // Select 3 random upgrades when the menu is enabled
        PickRandomUpgrades(3);
    }

    void PickRandomUpgrades(int numberToDisplay)
    {
        if (upgradeButtons.Length < numberToDisplay)
        {
            Debug.LogError("UpgradeDisplay: Not enough upgrade buttons to display!");
            return;
        }

        // Reset all upgrades to inactive
        foreach (var button in upgradeButtons)
        {
            button.SetActive(false);
        }

        // Create a shuffled list of indices
        List<int> indices = new List<int>();
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            indices.Add(i);
        }
        Shuffle(indices); // Randomize the indices

        int upgradesDisplayed = 0;
        foreach (int index in indices)
        {
            if (upgradesDisplayed >= numberToDisplay)
                break;

            // Get the LevelUpButtons component
            LevelUpButtons levelUpComponent = upgradeButtons[index].GetComponent<LevelUpButtons>();
            if (levelUpComponent == null)
            {
                Debug.LogError($"UpgradeDisplay: No LevelUpButtons component found on {upgradeButtons[index].name}!");
                continue;
            }

            // Skip upgrades that have reached max level
            if (levelUpComponent.ReachedMax)
                continue;

            // Activate the upgrade and increment the count
            upgradeButtons[index].SetActive(true);
            upgradesDisplayed++;
        }

        if (upgradesDisplayed < numberToDisplay)
        {
            Debug.LogWarning("UpgradeDisplay: Unable to find enough valid upgrades to display!");
        }
    }

    // Helper function to shuffle a list
    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(0, list.Count);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
