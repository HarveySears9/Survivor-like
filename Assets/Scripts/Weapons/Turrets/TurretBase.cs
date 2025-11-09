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


    protected virtual void Start()
    {
        lifeTimer = lifeTime;
    }

    protected virtual void Update()
    {
        HandleLifetime();

        // Firing cooldown logic (same as FireBreath)
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    protected virtual void HandleLifetime()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void Fire()
    {
        currentTarget = FindTargets();
        if (currentTarget == null) return;

        Vector2 fireDirection = (currentTarget.position - firePoint.position).normalized;

        // Instantiate projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Set rotation to match firing direction
        float angleToRotate = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleToRotate));

        // Send direction + damage to projectile (reuse Fireball or Weapon)
        projectile.GetComponent<Fireball>().Initialize(Vector2.right);
        projectile.GetComponent<Weapon>().damage = damage;
    }

    protected virtual Transform FindTargets()
    {
        List<Transform> enemyTargets = new List<Transform>();
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                enemyTargets.Add(hit.transform);
            }
        }

        if (enemyTargets.Count == 0) return null;

        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (Transform enemy in enemyTargets)
        {
            float dist = Vector2.Distance(transform.position, enemy.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = enemy;
            }
        }

        return closest;
    }

    public void LevelUp()
    {
        level++;
        if (level > maxLevel) level = maxLevel;

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
