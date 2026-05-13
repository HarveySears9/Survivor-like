using UnityEngine;

public static class PlayerStats
{
    public static int GetMaxHP()
    {
        var data = PlayerDataManager.Instance.data;

        float baseHP = 10f;
        float multiplier = 1f + (data.maxHPLevel * 0.2f);

        return Mathf.RoundToInt(baseHP * multiplier);
    }

    public static float GetDamageMultiplier()
    {
        var data = PlayerDataManager.Instance.data;

        return 1f + (data.damageLevel * 0.1f);
    }

    public static float GetSpeedMultiplier()
    {
        var data = PlayerDataManager.Instance.data;

        return 1f + (data.speedLevel * 0.05f);
    }

    public static float GetPickupRadius()
    {
        var data = PlayerDataManager.Instance.data;

        return 0.5f + (data.pickupRadiusLevel * 0.15f);
    }
}