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

    public SceneTransitionController stc;

    public void MainMenu()
    {
        if (Application.CanStreamedLevelBeLoaded("PuddleBrook"))
        {
            Time.timeScale = 1f;
            SceneTracker.UpdateLastSceneName();
            stc.TriggerTransition("PuddleBrook");
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

        MissionManager.Instance.AddProgress("time_Survived", Mathf.FloorToInt(gt.elapsedTime));

        MissionManager.Instance.AddProgress($"coins_Collected", pc.coins);

        timerText.text = $"Time Survived:\n{minutes:D2}:{seconds:D2}";

        killsText.text =
            "Enemies Defeated:\n" + KillCounter.enemyKills +
            "\nBosses Defeated:\n" + KillCounter.bossKills;
    }

    public void LevelComplete()
    {
        var data = PlayerDataManager.Instance.data;

        if (data == null)
            return;

        int currentLevel = PlayerPrefs.GetInt("SelectedLevel", 1);

        int nextLevelIndex = currentLevel;

        if (nextLevelIndex < data.levelsUnlocked.Length)
        {
            data.levelsUnlocked[nextLevelIndex] = true;
            PlayerDataManager.Instance.Save();
            Debug.Log("Unlocked Level " + (currentLevel + 1));
        }
        else
        {
            Debug.Log("Last level completed. Nothing to unlock.");
        }
    }
}
