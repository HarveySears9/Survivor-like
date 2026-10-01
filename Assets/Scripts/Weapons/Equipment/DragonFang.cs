using UnityEngine;

public class DragonFang : EquipmentBase
{
    [Header("Dragon Fang")]
    public float damageIncreasePerLevel = 0.10f;

    protected override void ApplyLevel()
    {
        if (player == null)
            return;

        player.equipmentDamageMultiplier =
            1f + (level * damageIncreasePerLevel);
    }
}