using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] characters;
    public GameObject Spawner;
    public GameObject tileMapPainter;
    
    void Start()
    {
        SetCharacter();
        Spawner.SetActive(true);
        tileMapPainter.SetActive(true);
    }

    private void SetCharacter()
    {
        characters[SaveFile.LoadData<SaveFile.Data>().currentCharacter].SetActive(true); 
        characters[SaveFile.LoadData<SaveFile.Data>().currentCharacter].tag = "Player";
    }
}
