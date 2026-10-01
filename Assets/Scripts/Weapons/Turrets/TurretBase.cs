using UnityEngine;
using System.Collections.Generic;

public class TurretBase : MonoBehaviour
{
    [Header("Turret Stats")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    public float fireRate = 1f;
    public float range = 4f;
    public float damage = 10f;

    public float lifeTime = 10f;

    public int level = 1;
    public int maxLevel = 5;

    protected float nextFireTime = 0f;
    protected float lifeTimer;

    protected Transform currentTarget;
    protected PlayerController player;

    protected virtual void Start()
    {
        player =
            FindObjectOfType<PlayerController>();

        lifeTimer = lifeTime;
    }

    protected virtual void Update()
    {
        HandleLifetime();

        if (Time.time >= nextFireTime)
        {
            Fire();

            nextFireTime =
                Time.time + GetTurretCooldown();
        }
    }

    // =========================
    // TURret MODIFIERS
    // =========================

    protected float GetTurretDamage()
    {
        float finalDamage =
            damage;

        // Permanent damage upgrade
        finalDamage *=
            PlayerStats.GetDamageMultiplier();

        // Temporary turret damage buffs
        if (player != null)
        {
            finalDamage *=
                player.turretDamageMultiplier;
        }

        return finalDamage;
    }

    protected float GetTurretCooldown()
    {
        float effectiveFireRate =
            fireRate;

        if (player != null)
        {
            effectiveFireRate *=
                player.attackSpeedMultiplier;
        }

        float cooldown =
            1f / effectiveFireRate;

        if (player != null)
        {
            cooldown *=
                player.turretCooldownMultiplier;
        }

        return cooldown;
    }

    // =========================
    // LIFETIME
    // =========================

    protected virtual void HandleLifetime()
    {
        lifeTimer -=
            Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    // =========================
    // FIRING
    // =========================

    protected virtual void Fire()
    {
        currentTarget =
            FindTargets();

        if (currentTarget == null)
            return;

        if (projectilePrefab == null ||
            firePoint == null)
            return;

        Vector2 fireDirection =
            (
                currentTarget.position -
                firePoint.position
            ).normalized;

        GameObject projectile =
            Instantiate(
                projectilePrefab,
                firePoint.position,
                Quaternion.identity
            );

        float angleToRotate =
            Mathf.Atan2(
                fireDirection.y,
                fireDirection.x
            ) * Mathf.Rad2Deg;

        projectile.transform.rotation =
            Quaternion.Euler(
                0f,
                0f,
                angleToRotate
            );

        Fireball fireball =
            projectile.GetComponent<Fireball>();

        if (fireball != null)
        {
            fireball.Initialize(
                Vector2.right
            );
        }

        Weapon weapon =
            projectile.GetComponent<Weapon>();

        if (weapon != null)
        {
            weapon.damage =
                GetTurretDamage();
        }
    }

    // =========================
    // TARGETING
    // =========================

    protected virtual Transform FindTargets()
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                transform.position,
                range
            );

        Transform closest = null;

        float closestDistance =
            Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            if (hit == null)
                continue;

            if (!hit.CompareTag("Enemy"))
                continue;

            float distance =
                Vector2.Distance(
                    transform.position,
                    hit.transform.position
                );

            if (distance < closestDistance)
            {
                closestDistance =
                    distance;

                closest =
                    hit.transform;
            }
        }

        return closest;
    }

    // =========================
    // LEVEL UP
    // =========================

    public virtual void LevelUp()
    {
        level++;

        if (level > maxLevel)
            level = maxLevel;

        switch (level)
        {
            case 2:
                break;

            case 3:
                break;

            case 4:
                damage *= 1.25f;
                break;

            case 5:
                fireRate *= 1.25f;
                break;
        }
    }
}