using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoots : MonoBehaviour
{
    public LevelUpButtons levelUpButton;

    public PlayerController player;

    public int level = 0;
    public int maxLevel = 5;

    public int percentIncreasePerLevel = 5;

    void Start()
    {
        levelUpButton.LevelUp(level, maxLevel);
    }

    public void LevelUp()
    {
        level++;
        if (level > maxLevel)
        {
            level = maxLevel;
        }

        levelUpButton.LevelUp(level, maxLevel);

        SetSpeed();
    }

    public void SetSpeed()
    {
        switch (level)
        {
            case 1:
                player.speed = player.startSpeed * (1f + (percentIncreasePerLevel / 100f));
                break;
            case 2:
                player.speed = player.startSpeed * (1f + (2 * percentIncreasePerLevel / 100f));
                break;
            case 3:
                player.speed = player.startSpeed * (1f + (3 * percentIncreasePerLevel / 100f));
                break;
            case 4:
                player.speed = player.startSpeed * (1f + (4 * percentIncreasePerLevel / 100f));
                break;
            case 5:
                player.speed = player.startSpeed * (1f + (5 * percentIncreasePerLevel / 100f));
                break;
            default:
                player.speed = player.startSpeed * (1f + (percentIncreasePerLevel / 100f));
                break;
        }
    }


}
