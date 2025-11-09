using UnityEngine;

public class IceTurret : TurretBase
{
    [Header("Ice Turret Parts")]
    public Transform barrel; // The rotating part of the turret

    protected override void Fire()
    {
        currentTarget = FindTargets();
        if (currentTarget == null) return;

        Vector2 fireDirection = (currentTarget.position - firePoint.position).normalized;

        // Rotate the barrel towards the target
        RotateBarrel(fireDirection);

        // Instantiate ice projectile
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Set rotation to match firing direction
        float angleToRotate = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleToRotate));

        // Send direction + damage to projectile
        if (projectile.TryGetComponent<Fireball>(out var fireball))
        {
            fireball.Initialize(Vector2.right);
        }

        if (projectile.TryGetComponent<Weapon>(out var weapon))
        {
            weapon.damage = damage;
        }
    }

    private void RotateBarrel(Vector2 direction)
    {
        if (barrel == null) return;

        // Rotate barrel
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        barrel.rotation = Quaternion.Euler(0, 0, angle);

        // Flip barrel on Y axis if facing left
        Vector3 localScale = barrel.localScale;
        if (angle > 90 || angle < -90)
        {
            localScale.y = -Mathf.Abs(localScale.y);
        }
        else
        {
            localScale.y = Mathf.Abs(localScale.y);
        }
        barrel.localScale = localScale;
    }
}
