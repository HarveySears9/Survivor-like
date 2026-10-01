using UnityEngine;

public abstract class EquipmentBase : MonoBehaviour
{
    [Header("Equipment")]
    public int level = 0;
    public int maxLevel = 5;

    public LevelUpButtons levelUpButton;

    protected virtual void Start()
    {
        if (levelUpButton != null)
        {
            levelUpButton.LevelUp(level, maxLevel);
        }
    }

    public virtual void LevelUp()
    {
        if (level >= maxLevel)
            return;

        level++;

        ApplyLevel();

        if (levelUpButton != null)
        {
            levelUpButton.LevelUp(level, maxLevel);
        }
    }

    protected abstract void ApplyLevel();
}