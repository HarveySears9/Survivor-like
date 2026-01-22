using UnityEngine;

public enum DragonDealType
{
    MaxHPIncrease,
    GoldGain,
    FreeLevel
}

[CreateAssetMenu(menuName = "Dragon Altar/Deal")]
public class DragonDeal : ScriptableObject
{
    [Header("Deal Type")]
    public DragonDealType dealType;

    [Header("Balance")]
    [Range(0.01f, 0.9f)]
    public float hpCostPercent;   // % of CURRENT HP (0.2 = 20%)

    public float value;           // meaning depends on dealType

    [Header("Spawn Weight")]
    [Min(1)]
    public int weight = 1;
}
