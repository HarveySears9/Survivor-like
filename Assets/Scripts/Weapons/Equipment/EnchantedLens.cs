using UnityEngine;

public class EnchantedLens : EquipmentBase
{
    [Header("Enchanted Lens")]
    public float sizeIncreasePerLevel = 0.10f;

    public SpinningBlades spinningBlades;
    public Hammer hammer;
    public DragonTail dragonTail;

    protected override void ApplyLevel()
    {
        if (player == null)
            return;

        player.areaSizeMultiplier =
            1f + (level * sizeIncreasePerLevel);

        if (spinningBlades != null)
            spinningBlades.UpdateAreaSize();
    }
}