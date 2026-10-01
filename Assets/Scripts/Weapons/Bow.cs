using UnityEngine;
using System.Collections;

public class Bow : WeaponBase
{
    [Header("Projectile")]
    public GameObject arrowPrefab;

    [Header("Burst")]
    public int arrowsPerBurst = 1;
    public float burstDelay = 0.12f;

    private bool isBursting = false;

    private Vector2 lastFireDirection =
        Vector2.right;

    public bool unlocked = true;

    public LevelUpButtons levelUpButton;

    protected override void Start()
    {
        base.Start();

        level = 0;

        unlocked =
            PlayerDataManager.Instance.data.weaponUnlocks[8];

        if (unlocked)
        {
            levelUpButton.LevelUp(
                level,
                maxLevel
            );
        }
    }

    void Update()
    {
        // Don't attack until the weapon
        // has actually been obtained
        if (level <= 0)
            return;

        // Remember the player's latest
        // movement direction
        if (player != null &&
            player.moveDirection != Vector2.zero)
        {
            lastFireDirection =
                player.moveDirection;
        }

        if (!isBursting && CooldownReady())
        {
            StartCoroutine(
                FireBurst()
            );
        }

        UpdateCooldownUI();
    }

    private IEnumerator FireBurst()
    {
        isBursting = true;

        for (int i = 0; i < arrowsPerBurst; i++)
        {
            Fire();

            if (i < arrowsPerBurst - 1)
            {
                yield return new WaitForSeconds(
                    burstDelay
                );
            }
        }

        // Burst is completely finished.
        // Start the cooldown now.
        StartCooldown();

        isBursting = false;
    }

    private void Fire()
    {
        if (player == null)
            return;

        Vector2 fireDirection =
            lastFireDirection;

        GameObject arrow =
            Instantiate(
                arrowPrefab,
                transform.position,
                Quaternion.identity
            );

        float angleToRotate =
            Mathf.Atan2(
                fireDirection.y,
                fireDirection.x
            ) * Mathf.Rad2Deg;

        arrow.transform.rotation =
            Quaternion.Euler(
                new Vector3(
                    0,
                    0,
                    angleToRotate
                )
            );

        Fireball fireball =
            arrow.GetComponent<Fireball>();

        if (fireball != null)
        {
            fireball.Initialize(
                Vector2.right
            );
        }

        Weapon weapon =
            arrow.GetComponent<Weapon>();

        if (weapon != null)
        {
            weapon.damage =
                GetWeaponDamage();
        }
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

        switch (level)
        {
            case 1:
                arrowsPerBurst = 1;
                break;

            case 2:
                arrowsPerBurst = 2;
                break;

            case 3:
                arrowsPerBurst = 3;
                break;

            case 4:
                arrowsPerBurst = 4;
                break;

            case 5:
                arrowsPerBurst = 5;
                break;
        }
    }
}