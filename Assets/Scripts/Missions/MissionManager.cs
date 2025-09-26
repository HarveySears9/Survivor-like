using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    [Header("Database Reference")]
    public MissionDatabase missionDatabase;

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

        DateTime now = DateTime.Now;

        // --- DAILY RESET ---
        // Next midnight after lastDailyReset
        DateTime nextDailyReset = lastDailyReset.Date.AddDays(1);

        if (now >= nextDailyReset || !data.activeMissions.Exists(m => m.type == ChallengeType.Daily))
        {
            // Remove any existing daily missions
            data.activeMissions.RemoveAll(m => m.type == ChallengeType.Daily);

            // Add 4 new daily missions from database
            AddRandomMissions(missionDatabase.dailyMissions, ChallengeType.Daily, 4, data.activeMissions);

            // Update reset times
            lastDailyReset = now.Date; // today at 00:00
            data.lastDailyReset = lastDailyReset;

            generateNewMissions = true;
        }

        // --- WEEKLY RESET ---
        // Find the Monday midnight after lastWeeklyReset
        int daysUntilMonday = ((int)DayOfWeek.Monday - (int)lastWeeklyReset.DayOfWeek + 7) % 7;
        DateTime nextWeeklyReset = lastWeeklyReset.Date.AddDays(daysUntilMonday == 0 ? 7 : daysUntilMonday);

        if (now >= nextWeeklyReset || !data.activeMissions.Exists(m => m.type == ChallengeType.Weekly))
        {
            // Remove any existing weekly missions
            data.activeMissions.RemoveAll(m => m.type == ChallengeType.Weekly);

            // Add 4 new weekly missions from database
            AddRandomMissions(missionDatabase.weeklyMissions, ChallengeType.Weekly, 4, data.activeMissions);

            // Update reset times (last Monday midnight)
            int daysSinceMonday = ((int)now.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            lastWeeklyReset = now.Date.AddDays(-daysSinceMonday);
            data.lastWeeklyReset = lastWeeklyReset;

            generateNewMissions = true;
        }

        if (generateNewMissions)
        {
            PlayerDataManager.Instance.Save();
        }
    }


    void AddRandomMissions(Mission[] sourceArray, ChallengeType type, int count, List<Mission> targetList)
    {
        if (sourceArray == null || sourceArray.Length == 0)
            return;

        // Make a copy of source array so we don’t modify the original
        List<Mission> pool = new List<Mission>(sourceArray);

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int index = UnityEngine.Random.Range(0, pool.Count);
            Mission chosen = new Mission
            {
                missionText = pool[index].missionText,
                key = pool[index].key,
                targetAmount = pool[index].targetAmount,
                rewardType = pool[index].rewardType,
                rewardAmount = pool[index].rewardAmount,
                type = type,
                completed = false,
                claimed = false,
                currentAmount = 0
            };

            targetList.Add(chosen);
            pool.RemoveAt(index);
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

    public void ClaimMission(Mission mission)
    {
        var data = PlayerDataManager.Instance.data;

        if (mission == null)
            return;

        if (!mission.completed || mission.claimed)
        {
            Debug.Log("Mission not claimable yet!");
            return;
        }

        // Mark as claimed
        mission.claimed = true;

        // Grant reward
        switch (mission.rewardType)
        {
            case RewardType.Coins:
                data.coins += mission.rewardAmount;
                Debug.Log($"Player gained {mission.rewardAmount} coins!");
                break;

            case RewardType.Skin:
                if (mission.rewardAmount >= 0 && mission.rewardAmount < data.skins.Length)
                {
                    data.skins[mission.rewardAmount].owned = true;
                    Debug.Log($"Player unlocked skin {mission.rewardAmount}!");
                }
                break;
        }

        // Save progress
        PlayerDataManager.Instance.Save();

        // Refresh UI
        FindObjectOfType<MissionUI>()?.RefreshMissionUI();
    }
}
