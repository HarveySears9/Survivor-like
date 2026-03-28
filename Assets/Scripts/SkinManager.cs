using UnityEngine;

public class SkinManager : MonoBehaviour
{
    [Header("Sprite Database")]
    public SpriteDatabase database;

    [Header("Scene References")]
    public AnimateSprite brickSp;           // Player sprite
    public AnimateImage loadingScreenImage;
    public AnimateImage[] upgradeButtons;   // Upgrade button sprites
    public AnimateSprite deadBrickSp;       // Dead player sprite

    private SaveFile.Data loadedData;

    void Start()
    {
        // Load saved data
        loadedData = PlayerDataManager.Instance.data;

        // Apply saved skin
        SetBrickSkin(loadedData.brickSkinEquipped);

        // Apply skin to upgrade buttons
        ApplySkinToUpgradeButtons();
    }

    /// Set B'rick's skin using the SpriteDatabase
    public void SetBrickSkin(int skinIndex)
    {
        switch (skinIndex)
        {
            case 0: AssignBrickSprites(database.red, database.redMoving, database.redDead); break;
            case 1: AssignBrickSprites(database.blue, database.blueMoving, database.blueDead); break;
            case 2: AssignBrickSprites(database.black, database.blackMoving, database.blackDead); break;
            case 3: AssignBrickSprites(database.gold, database.goldMoving, database.goldDead); break;
            case 4: AssignBrickSprites(database.teal, database.tealMoving, database.tealDead); break;
            case 5: AssignBrickSprites(database.bone, database.boneMoving, database.boneDead); break;
            case 6: AssignBrickSprites(database.suit, database.suitMoving, database.suitDead); break;
            case 7: AssignBrickSprites(database.blackBone, database.blackBoneMoving, database.blackBoneDead); break;
            case 8: AssignBrickSprites(database.whiteSuit, database.whiteSuitMoving, database.whiteSuitDead); break;
            case 9: AssignBrickSprites(database.blackSuit, database.blackSuitMoving, database.blackSuitDead); break;
            case 10: AssignBrickSprites(database.chef, database.chefMoving, database.chefDead); break;
            default:
                Debug.LogWarning("Invalid skin index for B'rick. Defaulting to Red.");
                AssignBrickSprites(database.red, database.redMoving, database.redDead);
                break;
        }
    }

    /// Assign sprites to the player and dead sprite
    private void AssignBrickSprites(Sprite[] sprites, Sprite[] moveArray, Sprite[] deadArray)
    {
        if (brickSp != null)
        {
            brickSp.spriteArray = sprites;
            brickSp.moveArray = moveArray;
        }

        if (loadingScreenImage != null)
        {
            loadingScreenImage.spriteArray = moveArray;
        }

        if (deadBrickSp != null)
        {
            deadBrickSp.spriteArray = deadArray;
        }
    }

    /// Apply the current skin to all upgrade buttons
    private void ApplySkinToUpgradeButtons()
    {
        foreach (var button in upgradeButtons)
        {
            button.spriteArray = brickSp.spriteArray;
        }
    }

    /// Change the equipped skin and save
    public void ChangeBrickSkin(int skinIndex)
    {
        loadedData.brickSkinEquipped = skinIndex;
        SetBrickSkin(skinIndex);
        SaveData();
    }

    /// Save the current data
    private void SaveData()
    {
        SaveFile.SaveData(loadedData);
    }
}
