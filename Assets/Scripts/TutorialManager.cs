using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private SaveFile.Data loadedData;

    public GameObject tutorialMenu;
    public GameObject joystick;

    void Start()
    {
        // Load saved data
        loadedData = PlayerDataManager.Instance.data;

        if (loadedData != null && !loadedData.tutorialCompleted)
        {
            StartCoroutine(StartTutorial());
        }
    }

    IEnumerator StartTutorial()
    {
        joystick.SetActive(false);

        yield return new WaitForSeconds(3f);

        tutorialMenu.SetActive(true);
        joystick.SetActive(true);
    }

    public void TutorialCompleted()
    {
        loadedData.tutorialCompleted = true;
        SaveFile.SaveData(loadedData);
    }
}