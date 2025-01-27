using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Library : MonoBehaviour
{
    public void Home()
    {
        if (Application.CanStreamedLevelBeLoaded("PuddleBrook"))
        {
            SceneTracker.UpdateLastSceneName();
            SceneManager.LoadScene("PuddleBrook");
        }
        else
        {
            Debug.LogError("Scene 'MainMenu' not found. Please check Build Settings.");
        }
    }
}
