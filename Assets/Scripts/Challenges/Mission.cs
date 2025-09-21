using UnityEngine;

public enum RewardType { Coins, Skin }
public enum ChallengeType { Daily, Weekly, Special }

[System.Serializable]
public class Mission
{
    public string missionText;       // e.g. "Defeat 100 Goblins"
    public RewardType rewardType;    // Coins for now
    public int rewardAmount;         // e.g. 100 coins
    public ChallengeType type;       // Daily or Weekly

    //public float Progress01 => Mathf.Clamp01((float)currentAmount / targetAmount);
}
