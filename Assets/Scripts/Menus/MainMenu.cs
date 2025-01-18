using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SaveFile.Data loadedData = SaveFile.LoadData<SaveFile.Data>();
        //loadedData.totalScore = 3000;
        //SaveFile.SaveData(loadedData);
        if (loadedData == null)
        {
            SaveFile.Data playerData = new SaveFile.Data();
            // Set initial values or load existing data into playerData
            SaveFile.SaveData(playerData);
        }
    }

    public void Play()
    {
        if (Application.CanStreamedLevelBeLoaded("Level1"))
        {
            SceneManager.LoadScene("Level1");
        }
        else
        {
            Debug.LogError("Scene 'Level1' not found. Please check Build Settings.");
        }
    }

}
