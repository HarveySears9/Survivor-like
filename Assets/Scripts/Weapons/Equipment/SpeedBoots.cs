using UnityEngine;

public class SpeedBoots : EquipmentBase
{
    [Header("Player")]
    public PlayerController player;

    [Header("Speed")]
    public int percentIncreasePerLevel = 5;

    [Header("Male Boots")]
    public Sprite[] boots1;
    public Sprite[] boots1moving;

    public Sprite[] boots2;
    public Sprite[] boots2moving;

    public Sprite[] boots3;
    public Sprite[] boots3moving;

    public Sprite[] boots4;
    public Sprite[] boots4moving;

    public Sprite[] boots5;
    public Sprite[] boots5moving;

    [Header("Female Boots")]
    public Sprite[] femaleBoots1;
    public Sprite[] femaleBoots1moving;

    public Sprite[] femaleBoots2;
    public Sprite[] femaleBoots2moving;

    public Sprite[] femaleBoots3;
    public Sprite[] femaleBoots3moving;

    public Sprite[] femaleBoots4;
    public Sprite[] femaleBoots4moving;

    public Sprite[] femaleBoots5;
    public Sprite[] femaleBoots5moving;

    [Header("Visuals")]
    public AnimateSprite gfx;
    public AnimateImage levelUpButtonAnimator;
    public SpriteRenderer sr;

    private bool isFemale = false;

    protected override void Start()
    {
        SaveFile.Data loadedData =
            SaveFile.LoadData<SaveFile.Data>();

        if (loadedData.currentCharacter != 0)
        {
            isFemale = true;
        }

        base.Start();
    }

    private void Update()
    {
        if (player == null)
            return;

        if (gfx != null)
        {
            gfx.isMoving =
                player.isMoving;
        }

        if (sr != null)
        {
            if (player.moveDirection.x < 0)
            {
                sr.flipX = true;
            }
            else if (player.moveDirection.x > 0)
            {
                sr.flipX = false;
            }
        }
    }

    protected override void ApplyLevel()
    {
        SetSpeed();
        UpdateSprites();
    }

    private void SetSpeed()
    {
        if (player == null)
            return;

        float multiplier =
            1f +
            (
                level *
                percentIncreasePerLevel /
                100f
            );

        player.speed =
            player.startSpeed * multiplier;
    }

    private void UpdateSprites()
    {
        switch (level)
        {
            case 1:

                if (!isFemale)
                {
                    gfx.spriteArray =
                        boots1;

                    gfx.moveArray =
                        boots1moving;

                    levelUpButtonAnimator.spriteArray =
                        boots2;
                }
                else
                {
                    gfx.spriteArray =
                        femaleBoots1;

                    gfx.moveArray =
                        femaleBoots1moving;

                    levelUpButtonAnimator.spriteArray =
                        femaleBoots2;
                }

                break;

            case 2:

                if (!isFemale)
                {
                    gfx.spriteArray =
                        boots2;

                    gfx.moveArray =
                        boots2moving;

                    levelUpButtonAnimator.spriteArray =
                        boots3;
                }
                else
                {
                    gfx.spriteArray =
                        femaleBoots2;

                    gfx.moveArray =
                        femaleBoots2moving;

                    levelUpButtonAnimator.spriteArray =
                        femaleBoots3;
                }

                break;

            case 3:

                if (!isFemale)
                {
                    gfx.spriteArray =
                        boots3;

                    gfx.moveArray =
                        boots3moving;

                    levelUpButtonAnimator.spriteArray =
                        boots4;
                }
                else
                {
                    gfx.spriteArray =
                        femaleBoots3;

                    gfx.moveArray =
                        femaleBoots3moving;

                    levelUpButtonAnimator.spriteArray =
                        femaleBoots4;
                }

                break;

            case 4:

                if (!isFemale)
                {
                    gfx.spriteArray =
                        boots4;

                    gfx.moveArray =
                        boots4moving;

                    levelUpButtonAnimator.spriteArray =
                        boots5;
                }
                else
                {
                    gfx.spriteArray =
                        femaleBoots4;

                    gfx.moveArray =
                        femaleBoots4moving;

                    levelUpButtonAnimator.spriteArray =
                        femaleBoots5;
                }

                break;

            case 5:

                if (!isFemale)
                {
                    gfx.spriteArray =
                        boots5;

                    gfx.moveArray =
                        boots5moving;
                }
                else
                {
                    gfx.spriteArray =
                        femaleBoots5;

                    gfx.moveArray =
                        femaleBoots5moving;
                }

                break;
        }
    }
}