using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;
    public SaveFile.Data data;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Load saved data (if it exists)
            data = SaveFile.LoadData<SaveFile.Data>();

            // Always define your defaults
            SkinData[] defaultSkins = new SkinData[]
            {
            new SkinData { owned = true, price = 0, achievement = true },      // default Red
            new SkinData { owned = false, price = 1000 },   // Blue
            new SkinData { owned = false, price = 5000 },   // Black
            new SkinData { owned = false, price = 7500 },   // Gold
            new SkinData { owned = false, price = 10000 },   // Teal
            new SkinData { owned = false, price = 0, achievement = true },       // Bone
            new SkinData { owned = false, price = 0},       // Suit
            new SkinData { owned = false, price = 0},       // Black Bone
            new SkinData { owned = false, price = 0},       // White Suit 
            new SkinData { owned = false, price = 0},       // Black Suit
            new SkinData { owned = false, price = 0, achievement = true}        // Chef
            };

            bool[] defaultLevels = new bool[] { true, true, true };
            bool[] weaponUnlocks = new bool[] { false };

            if (data == null)
            {
                // Create brand new data
                data = new SaveFile.Data();
                data.coins = 100000; // Testing
                data.maxHPLevel = 0;
                data.maxHP = 10;
                data.speedLevel = 0;
                data.currentDamage = 1;
                data.skins = defaultSkins;
                data.levelsUnlocked = defaultLevels;
                data.tutorialCompleted = false;
                data.weaponUnlocks = weaponUnlocks;
                Save();
            }
            else
            {
                // Merge skins
                int skinLengthToCopy = Mathf.Min(data.skins?.Length ?? 0, defaultSkins.Length);
                for (int i = 0; i < skinLengthToCopy; i++)
                {
                    defaultSkins[i].owned = data.skins[i].owned;
                }

                data.skins = defaultSkins;


                // Merge levels
                int levelLengthToCopy = Mathf.Min(data.levelsUnlocked?.Length ?? 0, defaultLevels.Length);
                for (int i = 0; i < levelLengthToCopy; i++)
                {
                    defaultLevels[i] = data.levelsUnlocked[i];
                }

                // ensure first level is always unlocked
                defaultLevels[0] = true;

                data.levelsUnlocked = defaultLevels;

                // Merge weapon unlocks
                int weaponLengthToCopy = Mathf.Min(data.weaponUnlocks?.Length ?? 0, weaponUnlocks.Length);
                for (int i = 0; i < levelLengthToCopy; i++)
                {
                    weaponUnlocks[i] = data.levelsUnlocked[i];
                }

                data.weaponUnlocks = weaponUnlocks;

                Save(); // overwrite with merged copy
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        SaveFile.SaveData(data);
    }
}
