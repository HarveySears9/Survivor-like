using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  // Required for TextMeshPro

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI coinText; // Correct reference type
    public GameObject skinManager;
    public SceneTransitionController stc;

    // Start is called before the first frame update
    void Start()
    {
        skinManager.SetActive(true);
    }

    // Method to load the next scene (Level1)
    public void Play()
    {
        if (Application.CanStreamedLevelBeLoaded("Level1"))
        {
            SceneTracker.UpdateLastSceneName();
            SceneManager.LoadScene("Level1");
        }
        else
        {
            Debug.LogError("Scene 'Level1' not found. Please check Build Settings.");
        }
    }

    public void Library()
    {
        if (Application.CanStreamedLevelBeLoaded("Library"))
        {
            SceneTracker.UpdateLastSceneName();
            SceneManager.LoadScene("Library");
        }
        else
        {
            Debug.LogError("Scene 'Library' not found. Please check Build Settings.");
        }
    }

    public void PuddleBrook()
    {
        if (Application.CanStreamedLevelBeLoaded("PuddleBrook"))
        {
            SceneTracker.UpdateLastSceneName();
            stc.TriggerTransition("PuddleBrook");
        }
        else
        {
            Debug.LogError("Scene 'Puddlebrook' not found. Please check Build Settings.");
        }
    }
}
