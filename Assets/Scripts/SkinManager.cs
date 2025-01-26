using System.Collections;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public Sprite[] red, redMoving, redDead, blue, blueMoving, blueDead, black, blackMoving, blackDead,
        kaelira, kaeliraMoving, kaeliraDead, kaeliraDress, kaeliraDressMoving, kaeliraDressDead,
        draven, dravenMoving, dravenDead;

    public AnimateSprite brickSp, kaeliraSp;             // References to the players' AnimateSprite
    public AnimateImage[] upgradeButtons;                // Array of upgrade button animations
    public AnimateSprite deadBrickSp, deadKaeliraSp;

    private SaveFile.Data loadedData;

    void Start()
    {
        // Load saved data
        loadedData = SaveFile.LoadData<SaveFile.Data>();
        if (loadedData == null)
        {
            loadedData = new SaveFile.Data();
            SaveData();
        }

        // Apply saved skins for both characters
        SetBrickSkin(loadedData.brickSkinEquipped);
        SetKaeliraSkin(loadedData.kaeliraSkinEquipped);


        ApplySkinToUpgradeButtons();
    }

    // Set the skin for B'rick
    void SetBrickSkin(int skinIndex)
    {
        switch (skinIndex)
        {
            case 0:
                brickSp.spriteArray = red;
                brickSp.moveArray = redMoving;
                if (deadBrickSp != null)
                {
                    deadBrickSp.spriteArray = redDead;
                }
                break;

            case 1:
                brickSp.spriteArray = blue;
                brickSp.moveArray = blueMoving;
                if (deadBrickSp != null)
                {
                    deadBrickSp.spriteArray = blueDead;
                }
                break;

            case 2:
                brickSp.spriteArray = black;
                brickSp.moveArray = blackMoving;
                if (deadBrickSp != null)
                {
                    deadBrickSp.spriteArray = blackDead;
                }
                break;

            case 5: // Draven
                brickSp.spriteArray = draven;
                brickSp.moveArray = dravenMoving;
                if (deadBrickSp != null)
                {
                    deadBrickSp.spriteArray = dravenDead;
                }
                break;

            default:
                Debug.LogWarning("Invalid skin index for B'rick. Defaulting to Red.");
                brickSp.spriteArray = red;
                brickSp.moveArray = redMoving;
                if (deadBrickSp != null)
                {
                    deadBrickSp.spriteArray = redDead;
                }
                break;
        }
    }

    // Set the skin for Kaelira
    void SetKaeliraSkin(int skinIndex)
    {
        switch (skinIndex)
        {
            case 3:
                kaeliraSp.spriteArray = kaelira;
                kaeliraSp.moveArray = kaeliraMoving;
                if (deadKaeliraSp != null)
                {
                    deadKaeliraSp.spriteArray = kaeliraDead;
                }
                break;

            case 4:
                kaeliraSp.spriteArray = kaeliraDress;
                kaeliraSp.moveArray = kaeliraDressMoving;
                if (deadKaeliraSp != null)
                {
                    deadKaeliraSp.spriteArray = kaeliraDressDead;
                }
                break;

            default:
                Debug.LogWarning("Invalid skin index for Kaelira. Defaulting to standard skin.");
                kaeliraSp.spriteArray = kaelira;
                kaeliraSp.moveArray = kaeliraMoving;
                if (deadKaeliraSp != null)
                {
                    deadKaeliraSp.spriteArray = kaeliraDead;
                }
                break;
        }
    }

    // Apply the current skin to all upgrade buttons
    void ApplySkinToUpgradeButtons()
    {
        switch (loadedData.currentCharacter)
        {
            case 0:
                foreach (var button in upgradeButtons)
                {
                    button.spriteArray = brickSp.spriteArray;
                }
                break;
            case 1:
                foreach (var button in upgradeButtons)
                {
                    button.spriteArray = kaeliraSp.spriteArray;
                }
                break;
        }
    }

    // Save the current data
    private void SaveData()
    {
        SaveFile.SaveData(loadedData);
    }

    // Change the current character
    public void SetCharacter(int characterIndex)
    {
        loadedData.currentCharacter = characterIndex;
        SaveData();
    }

    // Change the skin for the currently selected character
    public void ChangeSkin(int skinIndex)
    {
        switch (loadedData.currentCharacter)
        {
            case 0: // B'rick
                loadedData.brickSkinEquipped = skinIndex;
                SetBrickSkin(skinIndex);
                break;

            case 1: // Kaelira
                loadedData.kaeliraSkinEquipped = skinIndex;
                SetKaeliraSkin(skinIndex);
                break;

            default:
                Debug.LogWarning("Invalid character selected.");
                break;
        }

        SaveData();
    }
}
