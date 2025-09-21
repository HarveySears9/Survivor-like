using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MissionUIPrefab : MonoBehaviour
{
    public TextMeshProUGUI MissionText;
    public TextMeshProUGUI rewardAmount;
    public Image missionGFX;
    public Button claimButton;
    [Header("Progress UI")]
    public Slider progressSlider;      // Reference to the slider component
    public TextMeshProUGUI progressText;
}
