using UnityEngine;

public class Hourglass : EquipmentBase
{
    [Header("Hourglass")]
    public float cooldownMultiplierPerLevel = 0.9f;

    protected override void ApplyLevel()
    {
        if (player == null)
            return;

        player.weaponCooldownMultiplier =
            Mathf.Pow(cooldownMultiplierPerLevel, level);

        player.turretCooldownMultiplier =
            Mathf.Pow(cooldownMultiplierPerLevel, level);
    }
}