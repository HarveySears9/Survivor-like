using UnityEngine;

public class AcidSprayTurret : TurretBase
{
    [Header("Acid Spray Settings")]
    public GameObject spray;

    public SpriteRenderer[] spraySprites;

    [Header("Rotation Settings")]
    public Transform barrel;
    public float rotationSpeed = 360f;

    private float targetAngle = 0f;

    protected override void Update()
    {
        HandleLifetime();

        currentTarget = FindTargets();

        if (currentTarget != null)
        {
            if (spray != null &&
                !spray.activeSelf)
            {
                spray.SetActive(true);
            }

            Vector2 direction =
                (currentTarget.position -
                 barrel.position).normalized;

            targetAngle =
                Mathf.Atan2(
                    direction.y,
                    direction.x
                ) * Mathf.Rad2Deg;

            SmoothRotateBarrel();

            UpdateSprayDamage();
        }
        else
        {
            if (spray != null &&
                spray.activeSelf)
            {
                spray.SetActive(false);
            }
        }
    }

    private void UpdateSprayDamage()
    {
        if (spray == null)
            return;

        Weapon weapon =
            spray.GetComponent<Weapon>();

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

        SpriteRenderer barrelSR =
            barrel.GetComponent<SpriteRenderer>();

        if (barrelSR != null)
        {
            barrelSR.flipY =
                normalizedAngle > 90f ||
                normalizedAngle < -90f;
        }

        if (spraySprites != null &&
            spraySprites.Length > 0)
        {
            bool shouldFlip =
                normalizedAngle > 90f ||
                normalizedAngle < -90f;

            for (int i = 0;
                 i < spraySprites.Length;
                 i++)
            {
                if (spraySprites[i] != null)
                {
                    spraySprites[i].flipY =
                        shouldFlip;
                }
            }
        }
    }

    protected override void Fire()
    {
        // Acid spray deals continuous damage.
        // It does not use individual projectiles.
    }

    protected override void HandleLifetime()
    {
        base.HandleLifetime();

        if (spray != null)
        {
            spray.SetActive(false);
        }
    }
}