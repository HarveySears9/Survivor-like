using UnityEngine;

public class PlayerStaffOrbit : MonoBehaviour
{
    public GameObject poisonProjectilePrefab;

    public float orbitRadius = 1.2f;
    public float orbitSpeed = 180f;

    public float duration = 3f;
    public float fireInterval = 1f;
    public int projectileCount = 6;

    private Transform player;
    private PoisonStaff owner;

    private float angle;
    private float nextFireTime;
    public Transform staffGfx;
    public float spinSpeed = 360f;

    public void Initialize(Transform player, PoisonStaff owner, int level)
    {
        this.player = player;
        this.owner = owner;

        // Scale with level (simple for now)
        projectileCount = 1 + (level * 2);

        nextFireTime = Time.time;

        Destroy(gameObject, duration);
    }

    void Update()
    {
        if (player == null) return;

        // Orbit
        angle += orbitSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;

        Vector2 offset = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * orbitRadius;
        transform.position = (Vector2)player.position + offset;

        // Fire
        if (Time.time >= nextFireTime)
        {
            FireRadial();
            nextFireTime = Time.time + fireInterval;
        }

        // Spin the staff visual
        if (staffGfx != null)
        {
            staffGfx.Rotate(0f, 0f, spinSpeed * Time.deltaTime);
        }
    }

    void FireRadial()
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

    void OnDestroy()
    {
        owner.StaffFinished();
    }
}