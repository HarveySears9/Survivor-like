using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectMenu : MonoBehaviour
{
    private int selectedLevel = 1;
    public SceneTransitionController stc;

    private SaveFile.Data loadedData;

    public GameObject[] locks;
    public Button[] levelButtons;


    public void OnEnable()
    {
        loadedData = PlayerDataManager.Instance.data;

        if (loadedData != null)
        {
            for (int i = 0; i < levelButtons.Length; i++)
            {
                bool unlocked = loadedData.levelsUnlocked[i];

                // Show / hide lock
                locks[i].SetActive(!unlocked);

                // Enable / disable button clicking
                levelButtons[i].interactable = unlocked;
            }
        }
    }



    public void play()
    {
        if (Application.CanStreamedLevelBeLoaded("Level1"))
        {

            stc.TriggerTransition("Level1");
        }
    }

    public void SetLevel(int level)
    {
        selectedLevel = level;
        PlayerPrefs.SetInt("SelectedLevel", selectedLevel);
        PlayerPrefs.Save();
    }
}
