using UnityEngine;

public class StoneHeart : EquipmentBase
{
    [Header("Stone Heart")]
    public float maxHPIncreasePerLevel = 0.10f;

    protected override void ApplyLevel()
    {
        if (player == null)
            return;

        player.IncreaseMaxHP(maxHPIncreasePerLevel, true);
    }
}