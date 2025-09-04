using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownGate : MonoBehaviour
{
    public PlayButton playButton;
    public string name;
    public string sceneName;
    public Sprite openSprite, closeSprite;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D()
    {
        playButton.gameObject.SetActive(true);

        if (openSprite != null)
        {
            sr.sprite = openSprite;
        }

        playButton.SetUpButton(name, sceneName);
    }

    void OnTriggerExit2D()
    {
        if (playButton != null)
        {
            playButton.gameObject.SetActive(false);
        }

        if (closeSprite != null)
        {
            sr.sprite = closeSprite;
        }
    }
}
