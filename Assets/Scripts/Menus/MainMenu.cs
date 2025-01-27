using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  // Required for TextMeshPro

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI coinText; // Correct reference type
    public GameObject skinManager;

    // Start is called before the first frame update
    void Start()
    {
        SaveFile.Data loadedData = SaveFile.LoadData<SaveFile.Data>();

        // If no saved data, create a new one and save it
        if (loadedData == null)
        {
            SaveFile.Data playerData = new SaveFile.Data();

            playerData.coins = 999999;
            playerData.maxHPLevel = 0;
            playerData.maxHP = 10;
            playerData.currentDamage = 1;
            playerData.speedLevel = 0;
            playerData.brickSkinEquipped = 0;
            playerData.kaeliraSkinEquipped = 3;
            playerData.currentCharacter = 0;

            SaveFile.SaveData(playerData);
            loadedData = playerData; // Ensure loadedData is initialized
        }

        // Set coin text, ensuring loadedData is valid
        if (loadedData != null)
        {
            if(coinText != null)
            {
                coinText.text = "Coins:" + loadedData.coins.ToString();
            }
        }
        else
        {
            Debug.LogError("Failed to load player data.");
        }

        skinManager.SetActive(true);
    }

    // Method to load the next scene (Level1)
    public void Play()
    {
        if (Application.CanStreamedLevelBeLoaded("Level1"))
        {
            SceneTracker.UpdateLastSceneName();
            SceneManager.LoadScene("Level1");
        }
        else
        {
            Debug.LogError("Scene 'Level1' not found. Please check Build Settings.");
        }
    }

    public void Library()
    {
        if (Application.CanStreamedLevelBeLoaded("Library"))
        {
            SceneTracker.UpdateLastSceneName();
            SceneManager.LoadScene("Library");
        }
        else
        {
            Debug.LogError("Scene 'Library' not found. Please check Build Settings.");
        }
    }

    public void PuddleBrook()
    {
        if (Application.CanStreamedLevelBeLoaded("PuddleBrook"))
        {
            SceneTracker.UpdateLastSceneName();
            SceneManager.LoadScene("PuddleBrook");
        }
        else
        {
            Debug.LogError("Scene 'Puddlebrook' not found. Please check Build Settings.");
        }
    }
}
