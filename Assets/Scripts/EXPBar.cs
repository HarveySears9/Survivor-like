using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EXPBar : MonoBehaviour
{
    public Slider expBar;          // Reference to the EXP slider
    public int currentLevel = 1;   // Player's current level
    public float currentEXP = 0;   // Current EXP amount
    public float expThreshold = 100; // EXP needed for the next level
    public GameObject levelUpMenu; // Reference to the level-up menu

    public TextMeshProUGUI levelText;

    void Start()
    {
        UpdateEXPBar();
        SetText();
    }

    public void ScrollPickUp()
    {
        LevelUp(true);

        UpdateEXPBar();
    }

    // Call this when the player picks up a gem
    public void AddEXP(float amount)
    {
        currentEXP += amount;

        if (currentEXP >= expThreshold)
        {
            LevelUp(false);
        }

        UpdateEXPBar();
    }

    // Triggered when the player levels up
    void LevelUp(bool scroll)
    {
        currentLevel++; // Increase the player's level
        currentEXP -= expThreshold; // Carry over any extra EXP
        if(scroll)
        {
            currentEXP = 0;
        }
        expThreshold *= 1.25f; // Increase the threshold for the next level (optional)

        UpdateEXPBar();

        // Show the level-up menu and pause the game
        levelUpMenu.SetActive(true);
        SetText();
    }

    // Updates the slider to reflect the current EXP
    void UpdateEXPBar()
    {
        expBar.value = currentEXP / expThreshold; // Normalize the value (0 to 1)
    }

    void SetText()
    {
        levelText.text = "LV:"+currentLevel.ToString();
    }
}
