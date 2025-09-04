using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    private string scene;
    public GameObject levelSelectMenu;

    public void PushButton()
    {
        levelSelectMenu.SetActive(true);
        this.gameObject.SetActive(false);
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
