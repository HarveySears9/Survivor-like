using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathScreen : MonoBehaviour
{
    public PlayerController pc;
    public GameTimer gt;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI killsText;

    public void MainMenu()
    {
        if (Application.CanStreamedLevelBeLoaded("PuddleBrook"))
        {
            Time.timeScale = 1f;
            SceneTracker.UpdateLastSceneName();
            SceneManager.LoadScene("PuddleBrook");
        }
        else
        {
            Debug.LogError("Scene 'PuddleBrook' not found. Please check Build Settings.");
        }
    }

    void OnEnable()
    {
        coinText.text = "Coins Collected:\n" + pc.coins.ToString();

        int minutes = Mathf.FloorToInt(gt.elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(gt.elapsedTime % 60f);
        timerText.text = $"Time Survived:\n{minutes:D2}:{seconds:D2}";

        killsText.text =
            "Enemies Defeated:\n" + KillCounter.enemyKills +
            "\nBosses Defeated:\n" + KillCounter.bossKills;
    }
}
