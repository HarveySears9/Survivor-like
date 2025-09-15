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
            new SkinData { owned = true, price = 0 },      // default Red
            new SkinData { owned = false, price = 100 },   // Blue
            new SkinData { owned = false, price = 500 },   // Black
            new SkinData { owned = false, price = 750 },   // Gold
            new SkinData { owned = false, price = 1000 }   // Teal
            };

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
                Save();
            }
            else
            {
                // Merge ownership into defaults before saving
                int lengthToCopy = Mathf.Min(data.skins?.Length ?? 0, defaultSkins.Length);
                for (int i = 0; i < lengthToCopy; i++)
                {
                    defaultSkins[i].owned = data.skins[i].owned;
                }

                data.skins = defaultSkins;
                Save(); // overwrite with merged copy
            }

            // Debug log to check
            //foreach (var skin in data.skins)
            //{
            //    Debug.Log($"Owned: {skin.owned}, Price: {skin.price}");
            //}
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
