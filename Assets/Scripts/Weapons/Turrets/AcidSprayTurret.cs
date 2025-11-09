using UnityEngine;

public class AcidSprayTurret : TurretBase
{
    [Header("Acid Spray Settings")]
    public GameObject spray;           // The spray GameObject (already in scene, child of barrel)

    public SpriteRenderer[] spraySprites;

    [Header("Rotation Settings")]
    public Transform barrel;           // The rotating barrel/point of the turret
    public float rotationSpeed = 360f; // Degrees per second
    private float targetAngle = 0f;    // Angle to target

    protected override void Update()
    {
        HandleLifetime();
        // Find the nearest target
        currentTarget = FindTargets();

        if (currentTarget != null)
        {
            // Enable spray if it exists
            if (spray != null && !spray.activeSelf)
            {
                spray.SetActive(true);
            }

            // Calculate target angle toward enemy
            Vector2 direction = (currentTarget.position - barrel.position).normalized;
            targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Smoothly rotate barrel toward target and handle flips
            SmoothRotateBarrel();
        }
        else
        {
            // No target: disable spray
            if (spray != null && spray.activeSelf)
            {
                spray.SetActive(false);
            }
        }
    }

    private void SmoothRotateBarrel()
    {
        if (barrel == null) return;

        // Smooth rotation toward target angle
        float currentAngle = barrel.eulerAngles.z;
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        barrel.rotation = Quaternion.Euler(0, 0, newAngle);

        // Normalize angle to -180 to +180 for consistent flipping
        float normalizedAngle = (newAngle > 180f) ? newAngle - 360f : newAngle;

        // Flip the barrel sprite
        SpriteRenderer barrelSR = barrel.GetComponent<SpriteRenderer>();
        if (barrelSR != null)
        {
            barrelSR.flipY = (normalizedAngle > 90f || normalizedAngle < -90f);
        }

        // Flip the spray
        if (spraySprites != null && spraySprites.Length > 0)
        {
            bool shouldFlip = (normalizedAngle > 90f || normalizedAngle < -90f);
            for (int i = 0; i < spraySprites.Length; i++)
            {
                if (spraySprites[i] != null)
                    spraySprites[i].flipY = shouldFlip;
            }
        }

    }

    protected override void Fire()
    {
        // No individual projectiles; the spray handles continuous damage
    }

    protected override void HandleLifetime()
    {
        base.HandleLifetime();

        // Ensure spray is disabled if turret dies
        if (spray != null)
        {
            spray.SetActive(false);
        }
    }
}
