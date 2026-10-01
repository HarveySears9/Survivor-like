using UnityEngine;
using System.Collections;

public class DragonTail : WeaponBase
{
    [Header("Tail")]
    public GameObject tailPrefab;

    [Header("Attack")]
    public float attackDuration = 1.2f;

    private bool isAttacking = false;

    private Vector2 lastAttackDirection =
        Vector2.right;

    public bool unlocked = true;

    public LevelUpButtons levelUpButton;

    protected override void Start()
    {
        base.Start();

        level = 0;

        unlocked =
            PlayerDataManager.Instance.data.weaponUnlocks[9];

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

        // Remember the direction B'Rick is moving
        if (player != null &&
            player.moveDirection != Vector2.zero)
        {
            lastAttackDirection =
                player.moveDirection;
        }

        if (!isAttacking && CooldownReady())
        {
            StartCoroutine(
                Attack()
            );
        }

        UpdateCooldownUI();
    }

    private IEnumerator Attack()
    {
        isAttacking = true;

        if (player == null)
        {
            isAttacking = false;
            yield break;
        }

        // Create the tail as a child of B'Rick
        GameObject tail =
            Instantiate(
                tailPrefab,
                transform
            );

        // Apply Enchanted Lens area size
        tail.transform.localScale *=
            player.areaSizeMultiplier;

        // Position around B'Rick's waist / tail bone
        tail.transform.localPosition =
            new Vector3(
                0f,
                -1f,
                0f
            );

        // Apply weapon damage
        Weapon weapon =
            tail.GetComponent<Weapon>();

        if (weapon != null)
        {
            weapon.damage =
                GetWeaponDamage();
        }

        // Keep tail alive for the duration
        // of the attack animation
        float timer = 0f;

        while (timer < attackDuration)
        {
            // Update the direction while B'Rick
            // is moving
            if (player.moveDirection != Vector2.zero)
            {
                lastAttackDirection =
                    player.moveDirection;
            }

            // Attack opposite to B'Rick's
            // last movement direction
            Vector2 attackDirection =
                -lastAttackDirection;

            float angle =
                Mathf.Atan2(
                    attackDirection.y,
                    attackDirection.x
                ) * Mathf.Rad2Deg;

            // Tail sprite points UP by default,
            // so subtract 90 degrees
            tail.transform.localRotation =
                Quaternion.Euler(
                    0f,
                    0f,
                    angle - 90f
                );

            timer +=
                Time.deltaTime;

            yield return null;
        }

        Destroy(tail);

        isAttacking = false;

        // Start the cooldown AFTER
        // the attack has completely finished
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

        switch (level)
        {
            case 1:
                fireRate =
                    baseFireRate;
                break;

            case 2:
                fireRate =
                    baseFireRate * 1.15f;
                break;

            case 3:
                fireRate =
                    baseFireRate * 1.3f;
                break;

            case 4:
                fireRate =
                    baseFireRate * 1.45f;
                break;

            case 5:
                fireRate =
                    baseFireRate * 1.6f;
                break;
        }
    }
}