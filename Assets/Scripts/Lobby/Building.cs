using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public EnterButton enterButton;
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
        enterButton.gameObject.SetActive(true);

        if (openSprite != null)
        {
            sr.sprite = openSprite;
        }

        enterButton.SetUpButton(name, sceneName);
    }
    
    void OnTriggerExit2D()
    {
        if (enterButton != null)
        {
            enterButton.gameObject.SetActive(false);
        }

        if(closeSprite != null)
        {
            sr.sprite = closeSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
