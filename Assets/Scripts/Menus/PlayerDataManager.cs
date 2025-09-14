using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    public SaveFile.Data data; // Holds all player data (coins, skins, upgrades, etc.)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep across scenes

            // Load saved data or create default
            data = SaveFile.LoadData<SaveFile.Data>();
            if (data == null)
            {
                data = new SaveFile.Data();

                // Initialize defaults
                data.coins = 0;
                //Testing
                data.coins = 100000;

                data.maxHPLevel = 0;
                data.maxHP = 10; // starting HP
                data.speedLevel = 0;
                data.currentDamage = 1;
                data.currentCharacter = 0;

            }

            SkinData[] defaultSkins = new SkinData[]
            {
            new SkinData { owned = true, price = 0 },      // default Red
            new SkinData { owned = false, price = 100 },   // Blue
            new SkinData { owned = false, price = 500 },   // Black
            new SkinData { owned = false, price = 750 },   // Gold
            new SkinData { owned = false, price = 1000 }   // Teal
            };

            // Preserve ownership from the saved data
            if (data.skins != null)
            {
                int lengthToCopy = Mathf.Min(data.skins.Length, defaultSkins.Length);
                for (int i = 0; i < lengthToCopy; i++)
                {
                    defaultSkins[i].owned = data.skins[i].owned;
                }
            }

            data.skins = defaultSkins;

            Save(); // save initial data
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
