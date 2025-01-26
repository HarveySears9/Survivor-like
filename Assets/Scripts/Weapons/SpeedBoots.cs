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

    public Sprite[] femaleBoots1, femaleBoots1moving, femaleBoots2, femaleBoots2moving, femaleBoots3, femaleBoots3moving, femaleBoots4, femaleBoots4moving, femaleBoots5, femaleBoots5moving;

    private bool isFemale = false;

    public AnimateSprite gfx;
    public AnimateImage levelUpButtonAnimator;
    public SpriteRenderer sr;

    void Start()
    {
        SaveFile.Data loadedData = SaveFile.LoadData<SaveFile.Data>();

        // Check if the current character is not B'rick
        if (loadedData.currentCharacter != 0)
        {
            isFemale = true;
        }
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
                if (!isFemale)
                {
                    gfx.spriteArray = boots1;
                    gfx.moveArray = boots1moving;
                    levelUpButtonAnimator.spriteArray = boots2;
                }
                else
                {
                    gfx.spriteArray = femaleBoots1;
                    gfx.moveArray = femaleBoots1moving;
                    levelUpButtonAnimator.spriteArray = femaleBoots2;
                }
                break;
            case 2:
                if (!isFemale)
                {
                    gfx.spriteArray = boots2;
                    gfx.moveArray = boots2moving;
                    levelUpButtonAnimator.spriteArray = boots3;
                }
                else
                {
                    gfx.spriteArray = femaleBoots2;
                    gfx.moveArray = femaleBoots2moving;
                    levelUpButtonAnimator.spriteArray = femaleBoots3;
                }
                break;
            case 3:
                if (!isFemale)
                {
                    gfx.spriteArray = boots3;
                    gfx.moveArray = boots3moving;
                    levelUpButtonAnimator.spriteArray = boots4;
                }
                else
                {
                    gfx.spriteArray = femaleBoots3;
                    gfx.moveArray = femaleBoots3moving;
                    levelUpButtonAnimator.spriteArray = femaleBoots4;
                }
                break;
            case 4:
                if (!isFemale)
                {
                    gfx.spriteArray = boots4;
                    gfx.moveArray = boots4moving;
                    levelUpButtonAnimator.spriteArray = boots5;
                }
                else
                {
                    gfx.spriteArray = femaleBoots4;
                    gfx.moveArray = femaleBoots4moving;
                    levelUpButtonAnimator.spriteArray = femaleBoots5;
                }
                break;
            case 5:
                if (!isFemale)
                {
                    gfx.spriteArray = boots5;
                    gfx.moveArray = boots5moving;
                }
                else
                {
                    gfx.spriteArray = femaleBoots5;
                    gfx.moveArray = femaleBoots5moving;
                }
                break;
        }
    }
}
