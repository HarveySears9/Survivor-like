using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] characters;
    public GameObject[] levels;
    public DeathScreen deathScreen;

    public int currentLevel = 1;
    
    void Start()
    {
        SetCharacter();
        SetLevel(currentLevel);
    }

    private void SetCharacter()
    {
        characters[SaveFile.LoadData<SaveFile.Data>().currentCharacter].SetActive(true); 
        characters[SaveFile.LoadData<SaveFile.Data>().currentCharacter].tag = "Player";
        deathScreen.pc = characters[SaveFile.LoadData<SaveFile.Data>().currentCharacter].GetComponent<PlayerController>();
    }

    private void SetLevel(int level)
    {
        switch (level)
        {
            case 1:
                levels[0].SetActive(true);
                break;
            case 2:
                levels[1].SetActive(true);
                break;
            default:
                levels[0].SetActive(true);
                break;
        }
    }
}
