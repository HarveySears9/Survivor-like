using System;
using UnityEngine;

[Serializable]
public class MetaUpgrade
{
    public UpgradeType type;

    public string displayName;

    [TextArea]
    public string description;

    public int currentLevel;

    public int maxLevel = 5;

    public int[] costs;

    public float valuePerLevel;

    public bool IsMaxLevel()
    {
        return currentLevel >= maxLevel;
    }

    public int GetCurrentCost()
    {
        if (IsMaxLevel())
            return 0;

        return costs[currentLevel];
    }
}