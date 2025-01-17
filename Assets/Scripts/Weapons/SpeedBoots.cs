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

    public Sprite[] boots1, boots1moving, boots2, boots2moving, boots3, boots3moving, boots4, boots4moving, boots5, boots5moving;

    public AnimateSprite gfx;
    public AnimateImage levelUpButtonAnimator;
    public SpriteRenderer sr;

    void Start()
    {
        levelUpButton.LevelUp(level, maxLevel);
    }

    void Update()
    {
        gfx.isMoving = player.isMoving;

        // Flip the sprite's X-axis based on the player's movement direction
        if (player.moveDirection.x < 0)
        {
            sr.flipX = true; // Flip sprite when moving left
        }
        else if (player.moveDirection.x > 0)
        {
            sr.flipX = false; // Keep sprite normal when moving right
        }
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
        UpdateSprites();
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

    void UpdateSprites()
    {
        switch (level)
        {
            case 1:
                gfx.spriteArray = boots1;
                gfx.moveArray = boots1moving;
                levelUpButtonAnimator.spriteArray = boots2;
                break;
            case 2:
                gfx.spriteArray = boots2;
                gfx.moveArray = boots2moving;
                levelUpButtonAnimator.spriteArray = boots3;
                break;
            case 3:
                gfx.spriteArray = boots3;
                gfx.moveArray = boots3moving;
                levelUpButtonAnimator.spriteArray = boots4;
                break;
            case 4:
                gfx.spriteArray = boots4;
                gfx.moveArray = boots4moving;
                levelUpButtonAnimator.spriteArray = boots5;
                break;
            case 5:
                gfx.spriteArray = boots5;
                gfx.moveArray = boots5moving;
                break;
        }
    }
}
