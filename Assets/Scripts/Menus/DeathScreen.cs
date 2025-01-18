using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public void MainMenu()
    {
        if (Application.CanStreamedLevelBeLoaded("MainMenu"))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.LogError("Scene 'MainMenu' not found. Please check Build Settings.");
        }
    }
}
