using System.Collections.Generic;
using UnityEngine;

public class Hammer : WeaponBase
{
    [Header("Hammer")]
    public GameObject hammerPrefab;
    public float range = 10f;

    [Header("Player Animation")]
    public AnimateSprite playerAnimator;

    public Sprite[] hammer1;
    public Sprite[] hammer1moving;

    public Sprite[] hammer2;
    public Sprite[] hammer2moving;

    public Sprite[] hammer3;
    public Sprite[] hammer3moving;

    public Sprite[] hammer4;
    public Sprite[] hammer4moving;

    public Sprite[] hammer5;
    public Sprite[] hammer5moving;

    public LevelUpButtons levelUpButton;

    public SpriteRenderer sr;

    private bool hammerActive = false;

    public bool unlocked = false;

    protected override void Start()
    {
        SaveFile.Data loadedData =
            SaveFile.LoadData<SaveFile.Data>();

        unlocked =
            loadedData.weaponUnlocks[5];

        if (!unlocked)
            return;

        // Only enable for B'Rick
        if (loadedData.currentCharacter != 0)
        {
            gameObject.SetActive(false);
            return;
        }

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
            if (!hammerActive && CooldownReady())
            {
                ThrowAtTargets();
            }
        }

        if (player != null)
        {
            playerAnimator.isMoving =
                player.isMoving;

            // Flip sprite
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

    void ThrowAtTargets()
    {
        Collider2D[] targetsInRange =
            Physics2D.OverlapCircleAll(
                transform.position,
                range
            );

        List<Transform> enemyTargets =
            new List<Transform>();

        foreach (Collider2D col in targetsInRange)
        {
            if (col.CompareTag("Enemy"))
            {
                enemyTargets.Add(
                    col.transform
                );
            }
        }

        if (enemyTargets.Count == 0)
            return;

        Transform target =
            enemyTargets[0];

        GameObject hammerObj =
            Instantiate(
                hammerPrefab,
                transform.position,
                Quaternion.identity
            );

        PlayerHammer hammer =
            hammerObj.GetComponent<PlayerHammer>();

        if (hammer != null)
        {
            hammer.Initialize(
                target.position,
                transform,
                this,
                level,
                player.areaSizeMultiplier
            );

            float finalDamage =
                GetWeaponDamage();

            hammer.damage =
                finalDamage;

            Weapon hammerWeapon =
                hammerObj.GetComponent<Weapon>();

            if (hammerWeapon != null)
            {
                hammerWeapon.damage =
                    finalDamage;
            }
        }

        hammerActive = true;

        // Hide player's hammer while thrown
        sr.enabled = false;
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
                    hammer1;

                playerAnimator.moveArray =
                    hammer1moving;
                break;

            case 2:
                playerAnimator.spriteArray =
                    hammer2;

                playerAnimator.moveArray =
                    hammer2moving;
                break;

            case 3:
                playerAnimator.spriteArray =
                    hammer3;

                playerAnimator.moveArray =
                    hammer3moving;
                break;

            case 4:
                playerAnimator.spriteArray =
                    hammer4;

                playerAnimator.moveArray =
                    hammer4moving;
                break;

            case 5:
                playerAnimator.spriteArray =
                    hammer5;

                playerAnimator.moveArray =
                    hammer5moving;
                break;
        }
    }

    public void HammerReturned()
    {
        hammerActive = false;

        // Show player's hammer again
        sr.enabled = true;

        // Start cooldown when hammer returns
        StartCooldown();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            range
        );
    }

    void CreateWeaponUI()
    {
        GameObject uiObj =
            Instantiate(
                weaponUIPrefab,
                weaponUIParent
            );

        WeaponUI weaponUI =
            uiObj.GetComponent<WeaponUI>();

        weaponUI.icon.sprite =
            weaponIcon;

        weaponUI.cooldownSlider.enabled =
            false;
    }
}