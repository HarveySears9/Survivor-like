using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    public PlayerController pc;
    public GameTimer gt;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI overtimeBonusText;
    public TextMeshProUGUI killsText;

    public SceneTransitionController stc;

    public CutsceneData firstDeathCutscene;
    public CutsceneLoader cutsceneLoader;

    private bool coinsAdded = false;
    private int coinsFromRun;

    public Button doubleCoinsButton;
    public TextMeshProUGUI doubleCoinsButtonText;

    public bool isLevelComplete = false;
    public bool isOvertime = false;

    public void MainMenu()
    {
        Time.timeScale = 1f;

        // Check if the first-death cutscene has already been seen
        if (firstDeathCutscene != null &&
            cutsceneLoader != null &&
            !cutsceneLoader.HasCompletedCutscene(
                firstDeathCutscene.cutsceneID))
        {
            // Play the cutscene instead of going to PuddleBrook
            SceneTracker.SetLastSceneName("firstDeathCutscene");
            cutsceneLoader.PlayCutscene(firstDeathCutscene);
            return;
        }

        // Cutscene has already been seen, so go straight to PuddleBrook
        if (Application.CanStreamedLevelBeLoaded("PuddleBrook"))
        {
            SceneTracker.UpdateLastSceneName();
            stc.TriggerTransition("PuddleBrook");
        }
        else
        {
            Debug.LogError(
                "Scene 'PuddleBrook' not found. Please check Build Settings."
            );
        }
    }

    void OnEnable()
    {

        coinsFromRun = pc.coins;

        if (isOvertime && OvertimeManager.Instance != null)
        {
            int originalCoins = coinsFromRun;

            coinsFromRun = Mathf.RoundToInt(
                coinsFromRun * OvertimeManager.Instance.coinMultiplier
            );

            coinText.text =
                "Coins Collected:\n" + originalCoins.ToString();

            overtimeBonusText.text =
                "Overtime Bonus: x" +
                OvertimeManager.Instance.coinMultiplier.ToString("0.00") +
                "\nTotal: " + coinsFromRun.ToString();

            //overtimeBonusText.gameObject.SetActive(true);
        }
        else
        {
            coinText.text =
                "Coins Collected:\n" + coinsFromRun.ToString();

            overtimeBonusText.gameObject.SetActive(false);
        }

        int minutes = Mathf.FloorToInt(gt.elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(gt.elapsedTime % 60f);

        MissionManager.Instance.AddProgress(
            "time_Survived",
            Mathf.FloorToInt(gt.elapsedTime)
        );

        MissionManager.Instance.AddProgress(
            "complete_run",
            1
        );

        MissionManager.Instance.AddProgress(
            "coins_Collected",
            coinsFromRun
        );

        timerText.text =
            $"Time Survived:\n{minutes:D2}:{seconds:D2}";

        killsText.text =
            "Enemies Defeated:\n" + KillCounter.enemyKills +
            "\nBosses Defeated:\n" + KillCounter.bossKills;

        AddNormalCoins();
    }

    private void AddNormalCoins()
    {
        if (coinsAdded)
            return;

        var data = PlayerDataManager.Instance.data;

        data.coins += coinsFromRun;

        PlayerDataManager.Instance.Save();

        coinsAdded = true;
    }

    public void DoubleCoins()
    {
        if (AdsManager.Instance == null)
            return;

        doubleCoinsButton.interactable = false;

        bool adStarted = AdsManager.Instance.ShowRewardedAd(
            GiveDoubleCoinsReward
        );

        if (!adStarted)
        {
            // Ad wasn't available, so let the player try again
            doubleCoinsButton.interactable = true;
        }
    }

    private void GiveDoubleCoinsReward()
    {
        var data = PlayerDataManager.Instance.data;

        data.coins += coinsFromRun;

        PlayerDataManager.Instance.Save();

        if (isOvertime)
        {
            overtimeBonusText.text =
                "Overtime Bonus: x" +
                OvertimeManager.Instance.coinMultiplier.ToString("0.00") +
                "\nTotal: " + (coinsFromRun * 2).ToString();
        }
        else
        {
            coinText.text =
                "Coins Collected:\n" +
                (coinsFromRun * 2).ToString();
        }

        doubleCoinsButton.interactable = false;
        doubleCoinsButtonText.text = "Coins Doubled!";

        Debug.Log("Double Coins reward granted.");
    }

    public void LevelComplete()
    {
        isOvertime = true;

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
