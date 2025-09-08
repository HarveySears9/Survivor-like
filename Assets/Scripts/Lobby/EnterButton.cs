using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EnterButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    private string scene;

    public SceneTransitionController stc;

    // Start is called before the first frame update
    public void PushButton()
    {
        if (Application.CanStreamedLevelBeLoaded(scene))
        {
            stc.TriggerTransition(scene);
        }
        else
        {
            Debug.LogError("Scene not found. Please check Build Settings.");
        }
    }

    public void SetUpButton(string name, string scene)
    {
        SetText(name);
        this.scene = scene;
    }

    void SetText(string name)
    {
        buttonText.text = name;
    }
}
