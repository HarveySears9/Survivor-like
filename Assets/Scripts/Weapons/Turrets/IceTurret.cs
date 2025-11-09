using UnityEngine;

public class IceTurret : TurretBase
{
    [Header("Ice Turret Parts")]
    public Transform barrel; // The rotating part of the turret
    public float rotationSpeed = 360f; // Degrees per second
    public float aimTolerance = 5f; // Degrees within which it fires

    private float targetAngle;

    protected override void Update()
    {
        HandleLifetime();
        currentTarget = FindTargets();

        if (currentTarget != null)
        {
            Vector2 fireDirection = (currentTarget.position - firePoint.position).normalized;
            targetAngle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;

            // Smoothly rotate barrel toward target
            SmoothRotateBarrel();

            // Fire only if barrel is roughly aimed and cooldown passed
            float angleDifference = Mathf.DeltaAngle(barrel.eulerAngles.z, targetAngle);
            if (Mathf.Abs(angleDifference) <= aimTolerance && Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + 1f / fireRate;
            }
        }
    }

    protected override void Fire()
    {
        if (currentTarget == null) return;

        // Instantiate ice projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Set rotation to match firing direction
        projectile.transform.rotation = Quaternion.Euler(0, 0, targetAngle);

        // Send direction + damage to projectile
        if (projectile.TryGetComponent<Fireball>(out var fireball))
            fireball.Initialize(Vector2.right); // or fireDirection if Fireball expects a direction

        if (projectile.TryGetComponent<Weapon>(out var weapon))
            weapon.damage = damage;
    }

    private void SmoothRotateBarrel()
    {
        if (barrel == null) return;

        float currentAngle = barrel.eulerAngles.z;
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        barrel.rotation = Quaternion.Euler(0, 0, newAngle);

        // Normalize angle to -180 to +180 for consistent flipping
        float normalizedAngle = (newAngle > 180f) ? newAngle - 360f : newAngle;

        // Flip sprite if angle is pointing left
        SpriteRenderer sr = barrel.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.flipY = (normalizedAngle > 90f || normalizedAngle < -90f);
        }
    }

}
