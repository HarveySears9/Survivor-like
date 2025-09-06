using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelDisplay : MonoBehaviour
{
    public Sprite[] levelImages;
    public Image displayImage;
    public TextMeshProUGUI levelText;

    public void SetUpLevelDisplay(int level)
    {
        switch (level)
        {
            case 1:
                displayImage.sprite = levelImages[0];
                levelText.text = "Stage 1";
                break;
            case 2:
                displayImage.sprite = levelImages[1];
                levelText.text = "Stage 2";
                break;
            default:
                displayImage.sprite = levelImages[0];
                levelText.text = "Stage 1";
                break;
        }
    }
}