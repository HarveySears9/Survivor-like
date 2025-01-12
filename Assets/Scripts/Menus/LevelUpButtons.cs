using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpButtons : MonoBehaviour
{
    public TextMeshProUGUI levelText;        // Reference to the level text
    public Sprite[] levelImages;             // Array of images for each level
    public bool visibleUpgrade;             // Whether to show the upgrade image
    public Image displayImage;             // Current image being displayed
    private int currentLevel;
    private int max;

    public int CurrentLevel {  get { return currentLevel; } }

    public bool ReachedMax { get { return currentLevel == max; } }

    public void LevelUp(int level, int maxLevel )
    {
        // Validate the level and update the level text
        currentLevel = level;
        max = maxLevel;
        if (levelText != null)
        {
            if (level == maxLevel)
            {
                levelText.text = "LEVEL:\nMAX";
            }
            else if (level == maxLevel - 1)
            {
                levelText.text = "LEVEL:\n" + level.ToString() + " -> MAX";
            }
            else
            {
                levelText.text = "LEVEL:\n" + level.ToString() + " -> " + (level + 1).ToString();
            }
        }
        else
        {
            Debug.LogError("LevelUpButtons: Level text reference is missing!");
        }

        // Update the display image if visibleUpgrade is enabled
        if (visibleUpgrade && levelImages != null && level >= 0 && level < levelImages.Length)
        {
            if (displayImage != null)
            {
                displayImage.sprite = levelImages[level]; // Assign sprite from the array
            }
            else
            {
                Debug.LogError("LevelUpButtons: Display image reference is missing!");
            }
        }
    }
}
