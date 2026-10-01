using UnityEngine;

public class GoldenCoin : EquipmentBase
{
    [Header("Golden Coin")]
    public float bonusChancePerLevel = 0.10f;

    protected override void ApplyLevel()
    {
        if (player == null)
            return;

        player.coinBonusChance =
            level * bonusChancePerLevel;
    }
}