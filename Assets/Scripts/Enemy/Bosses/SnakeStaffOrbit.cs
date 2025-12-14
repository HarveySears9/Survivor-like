using UnityEngine;

public class SnakeStaffOrbit : MonoBehaviour
{
    public GameObject poisonProjectilePrefab;

    [Header("Orbit")]
    public float orbitRadius = 1.2f;
    public float orbitSpeed = 180f;

    [Header("Firing")]
    public float fireInterval = 0.8f;
    public int projectileCount = 12;

    [Header("Visual")]
    public Transform staffGfx;          // Child object (sprite)
    public float spinSpeed = 360f;      // Degrees per second

    private Transform boss;
    private float angle;
    private float nextFireTime;

    public void Initialize(Boss bossOwner, float duration)
    {
        boss = bossOwner.transform;
        nextFireTime = Time.time;
        Destroy(gameObject, duration);
    }

    void Update()
    {
        if (boss == null) return;

        // Orbit movement
        angle += orbitSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;

        Vector2 offset = new Vector2(
            Mathf.Cos(rad),
            Mathf.Sin(rad)
        ) * orbitRadius;

        transform.position = (Vector2)boss.position + offset;

        // Spin the staff visual
        if (staffGfx != null)
        {
            staffGfx.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
        }

        // Fire poison burst
        if (Time.time >= nextFireTime)
        {
            FireRadialPoison();
            nextFireTime = Time.time + fireInterval;
        }
    }

    void FireRadialPoison()
    {
        float step = 360f / projectileCount;

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = i * step;
            Vector2 dir = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );

            GameObject proj = Instantiate(poisonProjectilePrefab, transform.position, Quaternion.identity);
            proj.GetComponent<Boulder>().Initialize(dir);
        }
    }
}
