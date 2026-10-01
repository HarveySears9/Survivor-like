using UnityEngine;

public class IceTurret : TurretBase
{
    [Header("Ice Turret Parts")]
    public Transform barrel;
    public float rotationSpeed = 360f;
    public float aimTolerance = 5f;

    private float targetAngle;

    protected override void Update()
    {
        HandleLifetime();

        currentTarget = FindTargets();

        if (currentTarget != null)
        {
            Vector2 fireDirection =
                (currentTarget.position -
                 firePoint.position).normalized;

            targetAngle =
                Mathf.Atan2(
                    fireDirection.y,
                    fireDirection.x
                ) * Mathf.Rad2Deg;

            SmoothRotateBarrel();

            float angleDifference =
                Mathf.DeltaAngle(
                    barrel.eulerAngles.z,
                    targetAngle
                );

            if (Mathf.Abs(angleDifference) <= aimTolerance &&
                Time.time >= nextFireTime)
            {
                Fire();

                nextFireTime =
                    Time.time + GetTurretCooldown();
            }
        }
    }

    protected override void Fire()
    {
        if (currentTarget == null)
            return;

        GameObject projectile =
            Instantiate(
                projectilePrefab,
                firePoint.position,
                Quaternion.identity
            );

        projectile.transform.rotation =
            Quaternion.Euler(
                0f,
                0f,
                targetAngle
            );

        Fireball fireball =
            projectile.GetComponent<Fireball>();

        if (fireball != null)
        {
            fireball.Initialize(Vector2.right);
        }

        Weapon weapon =
            projectile.GetComponent<Weapon>();

        if (weapon != null)
        {
            weapon.damage =
                GetTurretDamage();
        }
    }

    private void SmoothRotateBarrel()
    {
        if (barrel == null)
            return;

        float currentAngle =
            barrel.eulerAngles.z;

        float newAngle =
            Mathf.MoveTowardsAngle(
                currentAngle,
                targetAngle,
                rotationSpeed * Time.deltaTime
            );

        barrel.rotation =
            Quaternion.Euler(
                0f,
                0f,
                newAngle
            );

        float normalizedAngle =
            (newAngle > 180f)
            ? newAngle - 360f
            : newAngle;

        SpriteRenderer sr =
            barrel.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.flipY =
                normalizedAngle > 90f ||
                normalizedAngle < -90f;
        }
    }
}