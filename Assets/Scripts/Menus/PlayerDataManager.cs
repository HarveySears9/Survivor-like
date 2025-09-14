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

                data.skins = new SkinData[]
                {
                    new SkinData { owned = true, price = 0 },
                    new SkinData { owned = false, price = 100 },
                    new SkinData { owned = false, price = 500 },
                    new SkinData { owned = false, price = 750 },
                    new SkinData { owned = false, price = 1000 }
                };

                data.maxHPLevel = 0;
                data.maxHP = 100; // starting HP
                data.speedLevel = 0;
                data.currentDamage = 1;

                Save(); // save initial data
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
