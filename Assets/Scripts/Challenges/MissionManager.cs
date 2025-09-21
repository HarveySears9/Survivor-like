using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    private DateTime lastDailyReset;
    private DateTime lastWeeklyReset;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadData();
        CheckAndGenerateMissions();
        DisplayMissions();
    }

    void LoadData()
    {
        var data = PlayerDataManager.Instance.data;

        // If no save exists, initialize
        if (data.activeMissions == null)
            data.activeMissions = new List<Mission>();

        lastDailyReset = data.lastDailyReset;
        lastWeeklyReset = data.lastWeeklyReset;
    }

    void CheckAndGenerateMissions()
    {
        var data = PlayerDataManager.Instance.data;

        bool generateNewMissions = false;

        // Daily reset
        if ((DateTime.Now - lastDailyReset).TotalHours >= 24 ||
        !data.activeMissions.Exists(m => m.type == ChallengeType.Daily))
        {
            // Remove any existing daily missions
            data.activeMissions.RemoveAll(m => m.type == ChallengeType.Daily);

            // Add new daily missions
            data.activeMissions.Add(new Mission
            {
                missionText = "Defeat 100 Goblins",
                targetAmount = 100, 
                key = "kill_Goblin",
                rewardType = RewardType.Coins,
                rewardAmount = 110,
                type = ChallengeType.Daily
            });

            data.activeMissions.Add(new Mission
            {
                missionText = "Defeat 50 Skeletons",
                targetAmount = 50,
                key = "kill_Skeleton",
                rewardType = RewardType.Coins,
                rewardAmount = 150,
                type = ChallengeType.Daily
            });

            data.activeMissions.Add(new Mission
            {
                missionText = "Survive for 2 minutes",
                targetAmount = 120,
                key = "time_Survived",
                rewardType = RewardType.Coins,
                rewardAmount = 150,
                type = ChallengeType.Daily
            });

            lastDailyReset = DateTime.Now;
            data.lastDailyReset = lastDailyReset;
            generateNewMissions = true;
        }

        // Weekly reset
        if ((DateTime.Now - lastWeeklyReset).TotalDays >= 7 ||
        !data.activeMissions.Exists(m => m.type == ChallengeType.Weekly))
        {
            // Remove any existing weekly missions
            data.activeMissions.RemoveAll(m => m.type == ChallengeType.Weekly);

            // Add new weekly missions
            data.activeMissions.Add(new Mission
            {
                missionText = "Defeat 1 Boss",
                targetAmount = 1,
                key = "kill_Boss",
                rewardType = RewardType.Coins,
                rewardAmount = 500,
                type = ChallengeType.Weekly
            });

            data.activeMissions.Add(new Mission
            {
                missionText = "Collect 200 Coins",
                targetAmount = 200,
                key = "coins_Collected",
                rewardType = RewardType.Coins,
                rewardAmount = 130,
                type = ChallengeType.Weekly
            });

            lastWeeklyReset = DateTime.Now;
            data.lastWeeklyReset = lastWeeklyReset;
            generateNewMissions = true;
        }

        if (generateNewMissions)
        {
            PlayerDataManager.Instance.Save();
        }
    }

    void DisplayMissions()
    {
        foreach (var mission in PlayerDataManager.Instance.data.activeMissions)
        {
            Debug.Log($"[{mission.type}] {mission.missionText} - Reward: {mission.rewardAmount} coins");
        }
    }

    public void AddProgress(string key, int amount = 1)
    {
        var data = PlayerDataManager.Instance.data;
        foreach (var mission in data.activeMissions)
        {
            if (mission.key == key && !mission.completed)
            {
                mission.currentAmount += amount;
                if (mission.currentAmount >= mission.targetAmount)
                {
                    mission.currentAmount = mission.targetAmount;
                    mission.completed = true;
                }
            }
        }

        PlayerDataManager.Instance.Save();
    }
}
