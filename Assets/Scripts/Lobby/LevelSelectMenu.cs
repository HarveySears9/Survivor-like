using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectMenu : MonoBehaviour
{
    private int selectedLevel = 1;

    public void play()
    {
        if (Application.CanStreamedLevelBeLoaded("Level1"))
        {
            SceneManager.LoadScene("Level1");
        }
    }

    public void SetLevel(int level)
    {
        selectedLevel = level;
        PlayerPrefs.SetInt("SelectedLevel", selectedLevel);
        PlayerPrefs.Save();
    }
}
