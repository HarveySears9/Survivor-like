using System.Collections;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public Sprite[] red, redMoving, blue, blueMoving, black, blackMoving, gold, goldMoving;

    public AnimateSprite player;             // Reference to the player's AnimateSprite
    public AnimateImage[] upgradeButtons;    // Array of upgrade button animations

    void Start()
    {   
        SaveFile.Data loadedData = SaveFile.LoadData<SaveFile.Data>();
        SetSkin(loadedData.skinEquipped);
        SaveFile.SaveData(loadedData);
    }

    void SetSkin(int skinIndex)
    {
        switch (skinIndex)
        {
            case 0: // Red Skin
                player.spriteArray = red;
                player.moveArray = redMoving;
                foreach (var button in upgradeButtons)
                {
                    button.spriteArray = red; // Set the sprite array for the upgrade buttons
                }
                break;

            case 1: // Blue Skin
                player.spriteArray = blue;
                player.moveArray = blueMoving;
                foreach (var button in upgradeButtons)
                {
                    button.spriteArray = blue;
                }
                break;

            case 2: // Black Skin
                player.spriteArray = black;
                player.moveArray = blackMoving;
                foreach (var button in upgradeButtons)
                {
                    button.spriteArray = black;
                }
                break;

            case 3: // Gold Skin
                player.spriteArray = gold;
                player.moveArray = goldMoving;
                foreach (var button in upgradeButtons)
                {
                    button.spriteArray = gold;
                }
                break;

            default: // Default Skin (Fallback to Red)
                player.spriteArray = red;
                player.moveArray = redMoving;
                foreach (var button in upgradeButtons)
                {
                    button.spriteArray = red;
                }
                break;
        }
    }


    public void ChangeSkin(int index)
    {
        SaveFile.Data loadedData = SaveFile.LoadData<SaveFile.Data>();
        loadedData.skinEquipped = index;
        SetSkin(loadedData.skinEquipped);
        SaveFile.SaveData(loadedData);
    }
}
