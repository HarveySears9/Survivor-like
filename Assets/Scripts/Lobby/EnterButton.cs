using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EnterButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    private string scene;
    private CutsceneData cutscene;

    //public SceneTransitionController stc;
    public CutsceneLoader cutsceneLoader;

    // Start is called before the first frame update
    public void PushButton()
    {
        if (Application.CanStreamedLevelBeLoaded(scene))
        {
            cutsceneLoader.PlayCutscene(cutscene);
        }
        else
        {
            Debug.LogError("Scene not found. Please check Build Settings.");
        }
    }

    public void SetUpButton(string name, string scene, CutsceneData cutscene)
    {
        SetText(name);
        this.scene = scene;
        this.cutscene = cutscene;
    }

    void SetText(string name)
    {
        buttonText.text = name;
    }
}
