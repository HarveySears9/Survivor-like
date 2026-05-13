using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrades/Upgrade Definition")]
public class UpgradeDefinition : ScriptableObject
{
    public UpgradeType type;

    public string displayName;

    [TextArea]
    public string description;

    public int maxLevel = 5;

    public int[] costs;

    [Header("Optional visual scaling info")]
    public float baseValue;
    public float valuePerLevel;

    public int GetCost(int currentLevel)
    {
        if (currentLevel >= maxLevel)
            return -1;

        return costs[currentLevel];
    }

    public bool IsMax(int level)
    {
        return level >= maxLevel;
    }
}