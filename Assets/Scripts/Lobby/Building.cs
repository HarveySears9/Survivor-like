using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public EnterButton enterButton;
    public string name;
    public string sceneName;

    void OnTriggerEnter2D()
    {
        enterButton.gameObject.SetActive(true);
        enterButton.SetUpButton(name, sceneName);
    }
    
    void OnTriggerExit2D()
    {
        enterButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
