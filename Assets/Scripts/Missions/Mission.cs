using UnityEngine;

public enum RewardType { Coins, Skin }
public enum ChallengeType { Daily, Weekly, Challenge }

[System.Serializable]
public class Mission
{
    public int id;            //for tracking Challenge missions existance
    public string missionText;       // e.g. "Defeat 100 Goblins"
    public RewardType rewardType;    // Coins or skins
    public string key;
    public int targetAmount;        // Amount if kills or seconds needed
    public int currentAmount = 0;
    public int rewardAmount;         // e.g. 100 coins or skin index
    public ChallengeType type;       // Daily or Weekly
    public bool claimed = false;
    public bool completed = false;

    public float Progress01 => Mathf.Clamp01((float)currentAmount / targetAmount);
}
