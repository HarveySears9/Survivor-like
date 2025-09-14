using UnityEngine;

public class SkinManager : MonoBehaviour
{
    [Header("Sprite Database")]
    public SpriteDatabase database;

    [Header("Scene References")]
    public AnimateSprite brickSp;           // Player sprite
    public AnimateImage[] upgradeButtons;   // Upgrade button sprites
    public AnimateSprite deadBrickSp;       // Dead player sprite

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
