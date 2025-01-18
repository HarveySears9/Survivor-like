using System.Collections;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public Sprite[] red, redMoving, redDead, blue, blueMoving, blueDead, black, blackMoving, blackDead, gold, goldMoving, goldDead;

    public AnimateSprite player;             // Reference to the player's AnimateSprite
    public AnimateImage[] upgradeButtons;    // Array of upgrade button animations
    public AnimateSprite deadPlayer;

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
                if(deadPlayer != null)
                {
                    deadPlayer.spriteArray = redDead;
                }

                break;

            case 1: // Blue Skin
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
                break;

            case 2: // Black Skin
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
                break;

            case 3: // Gold Skin
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
                break;

            default: // Default Skin (Fallback to Red)
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
