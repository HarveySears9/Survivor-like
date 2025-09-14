using System.Collections;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public Sprite[] red, redMoving, redDead, 
        blue, blueMoving, blueDead, 
        black, blackMoving, blackDead,
        gold, goldMoving, goldDead,
        teal, tealMoving, tealDead,
        kaelira, kaeliraMoving, kaeliraDead, 
        kaeliraDress, kaeliraDressMoving, kaeliraDressDead, 
        kaeliraRed, kaeliraRedMoving, kaeliraRedDead, 
        kaeliraGreen, kaeliraGreenMoving, kaeliraGreenDead,
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

        switch (loadedData.currentCharacter)
        {
            case 0:
                SetBrickSkin(loadedData.brickSkinEquipped);
                break;
            case 1:
                SetKaeliraSkin(loadedData.kaeliraSkinEquipped);
                break;
        }


        ApplySkinToUpgradeButtons();
    }

    // Set the skin for B'rick
    void SetBrickSkin(int skinIndex)
    {
        switch (skinIndex)
        {
            case 0: // Default Red
                brickSp.spriteArray = red;
                brickSp.moveArray = redMoving;
                if (deadBrickSp != null)
                {
                    deadBrickSp.spriteArray = redDead;
                }
                break;

            case 1: //Blue
                brickSp.spriteArray = blue;
                brickSp.moveArray = blueMoving;
                if (deadBrickSp != null)
                {
                    deadBrickSp.spriteArray = blueDead;
                }
                break;

            case 2: //Black
                brickSp.spriteArray = black;
                brickSp.moveArray = blackMoving;
                if (deadBrickSp != null)
                {
                    deadBrickSp.spriteArray = blackDead;
                }
                break;

            case 3: // Gold
                brickSp.spriteArray = draven;
                brickSp.moveArray = dravenMoving;
                if (deadBrickSp != null)
                {
                    deadBrickSp.spriteArray = dravenDead;
                }
                break;
            case 4: // Teal
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

            case 6:
                kaeliraSp.spriteArray = kaeliraRed;
                kaeliraSp.moveArray = kaeliraRedMoving;
                if (deadKaeliraSp != null)
                {
                    deadKaeliraSp.spriteArray = kaeliraRedDead;
                }
                break;

            case 7:
                kaeliraSp.spriteArray = kaeliraGreen;
                kaeliraSp.moveArray = kaeliraGreenMoving;
                if (deadKaeliraSp != null)
                {
                    deadKaeliraSp.spriteArray = kaeliraGreenDead;
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
    public void ChangeBrickSkin(int skinIndex)
    {
        loadedData.brickSkinEquipped = skinIndex;
        SetBrickSkin(skinIndex);
        SaveData();
    }

    // Change the skin for the currently selected character
    public void ChangeKaeliraSkin(int skinIndex)
    {
        loadedData.kaeliraSkinEquipped = skinIndex;
        SetKaeliraSkin(skinIndex);
        SaveData();
    }
}
