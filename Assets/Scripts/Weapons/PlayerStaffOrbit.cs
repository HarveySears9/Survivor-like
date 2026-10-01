using UnityEngine;

public class PlayerStaffOrbit : MonoBehaviour
{
    [Header("Projectiles")]
    public GameObject poisonProjectilePrefab;

    [Header("Orbit")]
    public float orbitRadius = 1.2f;
    public float orbitSpeed = 180f;

    [Header("Attack")]
    public float duration = 3f;
    public float fireInterval = 1f;
    public int projectileCount = 6;

    [Header("Visual")]
    public Transform staffGfx;
    public float spinSpeed = 360f;

    private Transform player;
    private PoisonStaff owner;

    private float angle;
    private float nextFireTime;

    private float damage;

    private bool finished = false;

    public void Initialize(
        Transform player,
        PoisonStaff owner,
        int level,
        float damage
    )
    {
        this.player = player;
        this.owner = owner;
        this.damage = damage;

        // Increase projectile count with weapon level
        projectileCount =
            1 + (level * 2);

        nextFireTime =
            Time.time;

        Destroy(
            gameObject,
            duration
        );
    }

    void Update()
    {
        if (player == null)
            return;

        // Orbit around the player
        angle +=
            orbitSpeed *
            Time.deltaTime;

        float rad =
            angle *
            Mathf.Deg2Rad;

        Vector2 offset =
            new Vector2(
                Mathf.Cos(rad),
                Mathf.Sin(rad)
            ) *
            orbitRadius;

        transform.position =
            (Vector2)player.position +
            offset;

        // Fire poison projectiles
        if (Time.time >= nextFireTime)
        {
            FireRadial();

            nextFireTime =
                Time.time +
                fireInterval;
        }

        // Rotate staff graphic
        if (staffGfx != null)
        {
            staffGfx.Rotate(
                0f,
                0f,
                spinSpeed *
                Time.deltaTime
            );
        }
    }

    void FireRadial()
    {
        if (poisonProjectilePrefab == null)
            return;

        if (projectileCount <= 0)
            return;

        float step =
            360f /
            projectileCount;

        for (int i = 0; i < projectileCount; i++)
        {
            float projectileAngle =
                i * step;

            Vector2 direction =
                new Vector2(
                    Mathf.Cos(
                        projectileAngle *
                        Mathf.Deg2Rad
                    ),
                    Mathf.Sin(
                        projectileAngle *
                        Mathf.Deg2Rad
                    )
                );

            GameObject projectile =
                Instantiate(
                    poisonProjectilePrefab,
                    transform.position,
                    Quaternion.identity
                );

            Boulder boulder =
                projectile.GetComponent<Boulder>();

            if (boulder != null)
            {
                boulder.Initialize(
                    direction
                );
            }

            Weapon weapon =
                projectile.GetComponent<Weapon>();

            if (weapon != null)
            {
                weapon.damage =
                    damage;
            }
        }
    }

    void OnDestroy()
    {
        if (finished)
            return;

        finished = true;

        if (owner != null)
        {
            owner.StaffFinished();
        }
    }
}