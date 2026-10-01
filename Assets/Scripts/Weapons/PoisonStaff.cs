using System.Collections.Generic;
using UnityEngine;

public class PoisonStaff : WeaponBase
{
    [Header("Poison Staff")]
    public GameObject staffOrbitPrefab;

    [Header("Player Animation")]
    public AnimateSprite playerAnimator;

    public Sprite[] staff1;
    public Sprite[] staff1moving;

    public Sprite[] staff2;
    public Sprite[] staff2moving;

    public Sprite[] staff3;
    public Sprite[] staff3moving;

    public Sprite[] staff4;
    public Sprite[] staff4moving;

    public Sprite[] staff5;
    public Sprite[] staff5moving;

    public LevelUpButtons levelUpButton;

    public SpriteRenderer sr;

    private bool staffActive = false;

    public bool unlocked = false;

    protected override void Start()
    {
        SaveFile.Data loadedData =
            SaveFile.LoadData<SaveFile.Data>();

        unlocked =
            loadedData.weaponUnlocks[6];

        if (!unlocked)
            return;

        base.Start();

        level = 0;

        levelUpButton.LevelUp(
            level,
            maxLevel
        );

        UpdateSprites();
    }

    void Update()
    {
        if (level > 0)
        {
            if (!staffActive && CooldownReady())
            {
                SpawnStaff();
            }
        }

        if (player != null)
        {
            playerAnimator.isMoving =
                player.isMoving;

            if (player.moveDirection.x < 0)
            {
                sr.flipX = true;
            }
            else if (player.moveDirection.x > 0)
            {
                sr.flipX = false;
            }
        }

        UpdateCooldownUI();
    }

    void SpawnStaff()
    {
        GameObject staff =
            Instantiate(
                staffOrbitPrefab,
                transform.position,
                Quaternion.identity
            );

        PlayerStaffOrbit orbit =
            staff.GetComponent<PlayerStaffOrbit>();

        if (orbit != null)
        {
            orbit.Initialize(
                transform,
                this,
                level,
                GetWeaponDamage()
            );
        }

        staffActive = true;

        // Hide held staff while the orbiting staff is active
        sr.enabled = false;
    }

    public void StaffFinished()
    {
        staffActive = false;

        // Show held staff again
        sr.enabled = true;

        // Cooldown begins after the orbiting staff finishes
        StartCooldown();
    }

    public override void LevelUp()
    {
        level++;

        if (level > maxLevel)
            level = maxLevel;

        if (level == 1)
        {
            CreateWeaponUI();
        }

        levelUpButton.LevelUp(
            level,
            maxLevel
        );

        UpdateSprites();
    }

    void UpdateSprites()
    {
        switch (level)
        {
            case 1:
                playerAnimator.spriteArray =
                    staff1;

                playerAnimator.moveArray =
                    staff1moving;
                break;

            case 2:
                playerAnimator.spriteArray =
                    staff2;

                playerAnimator.moveArray =
                    staff2moving;
                break;

            case 3:
                playerAnimator.spriteArray =
                    staff3;

                playerAnimator.moveArray =
                    staff3moving;
                break;

            case 4:
                playerAnimator.spriteArray =
                    staff4;

                playerAnimator.moveArray =
                    staff4moving;
                break;

            case 5:
                playerAnimator.spriteArray =
                    staff5;

                playerAnimator.moveArray =
                    staff5moving;
                break;
        }
    }
}