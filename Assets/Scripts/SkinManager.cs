using System.Collections;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public Sprite[] red, redMoving, redDead, blue, blueMoving, blueDead, black, blackMoving, blackDead, gold, goldMoving, goldDead, draven, dravenMoving, dravenDead;

    public AnimateSprite player;             // Reference to the player's AnimateSprite
    public AnimateImage[] upgradeButtons;    // Array of upgrade button animations
    public AnimateSprite deadPlayer;

    private SaveFile.Data loadedData;

    void Start()
    {   
        loadedData = SaveFile.LoadData<SaveFile.Data>();
        SetSkin(loadedData.skinEquipped);
        SaveFile.SaveData(loadedData);
    }

    void SetSkin(int skinIndex)
    {

        switch (skinIndex)
        {
            case 0: // Red Skin (B'rick)
                player.spriteArray = red;
                player.moveArray = redMoving;
                foreach (var button in upgradeButtons)
                {
                    button.spriteArray = red; // Set the sprite array for the upgrade buttons
                }
                if (deadPlayer != null)
                {
                    deadPlayer.spriteArray = redDead;
                }
                loadedData.currentCharacter = 0; // B'rick
                break;

            case 1: // Blue Skin (B'rick)
                player.spriteArray = blue;
                player.moveArray = blueMoving;
                foreach (var button in upgradeButtons)
                {
                    button.spriteArray = blue;
                }
                if (deadPlayer != null)
                {
                    deadPlayer.spriteArray = blueDead;
                }
                loadedData.currentCharacter = 0; // B'rick
                break;

            case 2: // Black Skin (B'rick)
                player.spriteArray = black;
                player.moveArray = blackMoving;
                foreach (var button in upgradeButtons)
                {
                    button.spriteArray = black;
                }
                if (deadPlayer != null)
                {
                    deadPlayer.spriteArray = blackDead;
                }
                loadedData.currentCharacter = 0; // B'rick
                break;

            case 3: // Gold Skin (B'rick)
                player.spriteArray = gold;
                player.moveArray = goldMoving;
                foreach (var button in upgradeButtons)
                {
                    button.spriteArray = gold;
                }
                if (deadPlayer != null)
                {
                    deadPlayer.spriteArray = goldDead;
                }
                loadedData.currentCharacter = 0; // B'rick
                break;

            case 4: // Draven Skin
                player.spriteArray = draven;
                player.moveArray = dravenMoving;
                foreach (var button in upgradeButtons)
                {
                    button.spriteArray = draven;
                }
                if (deadPlayer != null)
                {
                    deadPlayer.spriteArray = dravenDead;
                }
                loadedData.currentCharacter = 1; // Draven
                break;

            default: // Default Skin (Fallback to Red/B'rick)
                player.spriteArray = red;
                player.moveArray = redMoving;
                foreach (var button in upgradeButtons)
                {
                    button.spriteArray = red;
                }
                if (deadPlayer != null)
                {
                    deadPlayer.spriteArray = redDead;
                }
                loadedData.currentCharacter = 0; // B'rick
                break;
        }

        // Save the updated character data
        SaveFile.SaveData(loadedData);
    }


    public void ChangeSkin(int index)
    {
        loadedData.skinEquipped = index; // Update skinEquipped in loadedData
        SetSkin(loadedData.skinEquipped); // Update the skin
        Debug.Log("Current Character: " + loadedData.currentCharacter); // Debug log to verify
        SaveFile.SaveData(loadedData); // Save the data
    }
}
