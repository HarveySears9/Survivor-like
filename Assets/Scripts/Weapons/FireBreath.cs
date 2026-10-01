using UnityEngine;
using System.Collections.Generic;

public class FireBreath : WeaponBase
{
    [Header("Fire Breath")]
    public GameObject fireballPrefab;
    public float range = 10f;

    public LevelUpButtons levelUpButton;


    protected override void Start()
    {
        base.Start();

        level = 0;

        levelUpButton.LevelUp(level, maxLevel);
    }


    void Update()
    {
        // Don't attack until the weapon has actually been obtained.
        if (level <= 0)
            return;

        if (CooldownReady())
        {
            if (Fire())
            {
                StartCooldown();
            }
        }

        UpdateCooldownUI();
    }


    private bool Fire()
    {
        Transform enemyTarget = FindTargets();

        // No target, so don't start the cooldown.
        if (enemyTarget == null)
            return false;

        Vector2 fireDirection =
            (enemyTarget.position - transform.position).normalized;

        GameObject fireball = Instantiate(
            fireballPrefab,
            transform.position,
            Quaternion.identity
        );

        // Make the fireball larger
        fireball.transform.localScale *= 1.5f;

        float angleToRotate =
            Mathf.Atan2(
                fireDirection.y,
                fireDirection.x
            ) * Mathf.Rad2Deg;

        fireball.transform.rotation =
            Quaternion.Euler(0, 0, angleToRotate);

        fireball.GetComponent<Fireball>()
            .Initialize(Vector2.right);

        fireball.GetComponent<Weapon>().damage =
            GetWeaponDamage();

        return true;
    }


    private Transform FindTargets()
    {
        List<Transform> enemyTargets =
            new List<Transform>();

        Collider2D[] targetsInRange =
            Physics2D.OverlapCircleAll(
                transform.position,
                range
            );

        foreach (var target in targetsInRange)
        {
            if (target.CompareTag("Enemy"))
            {
                enemyTargets.Add(target.transform);
            }
        }

        if (enemyTargets.Count == 0)
            return null;

        Transform closestTarget = null;

        float closestDistance =
            Mathf.Infinity;

        foreach (Transform enemy in enemyTargets)
        {
            float distance =
                Vector2.Distance(
                    transform.position,
                    enemy.position
                );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = enemy;
            }
        }

        return closestTarget;
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

        levelUpButton.LevelUp(level, maxLevel);

        switch (level)
        {
            case 1:
                fireRate = baseFireRate;
                break;

            case 2:
                fireRate = baseFireRate * 1.4f;
                break;

            case 3:
                fireRate = baseFireRate * 1.8f;
                break;

            case 4:
                fireRate = baseFireRate * 2.3f;
                break;

            case 5:
                fireRate = baseFireRate * 3f;
                break;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            range
        );
    }
}