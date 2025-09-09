using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Library : MonoBehaviour
{
    public SceneTransitionController stc;
    public GameObject[] tabs;

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

    public void OnTabPressed(int index)
    {
        foreach (var tab in tabs)
        {
            tab.SetActive(false);
        }
        tabs[index].SetActive(true);
    }
}
