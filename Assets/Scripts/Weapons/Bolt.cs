using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class Bolt : MonoBehaviour
{
    public GameObject gfx;
    public float speed = 10f;
    public float lifetime = 3f; // seconds before self-destroy
    public float hitRadius = 0.4f; // size of trigger collider

    private Transform target;
    private float damage;
    private int chainsRemaining;
    private float chainRange;
    private bool hasHit = false;

    private List<Transform> hitEnemies = new List<Transform>();

    void Awake()
    {
        // Ensure the collider is a trigger
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
        col.offset = Vector2.zero;
        col.usedByEffector = false;
        if (col is CircleCollider2D circle)
            circle.radius = hitRadius;
    }

    public void Initialize(Transform firstTarget, float dmg, int chains, float range, List<Transform> previousHits = null)
    {
        target = firstTarget;
        damage = dmg;
        chainsRemaining = chains;
        chainRange = range;

        if (previousHits != null)
            hitEnemies = new List<Transform>(previousHits);
        else
            hitEnemies = new List<Transform>();

        if (firstTarget != null && !hitEnemies.Contains(firstTarget))
            hitEnemies.Add(firstTarget);
    }

    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
            return;
        }

        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Move toward target
        Vector2 dir = (target.position - transform.position).normalized;
        gfx.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;
        if (!other.CompareTag("Enemy")) return;
        if (other.transform != target) return;

        hasHit = true;
        HitTarget();
    }

    void HitTarget()
    {
        if (!hitEnemies.Contains(target))
            hitEnemies.Add(target);

        Debug.Log($"⚡ Bolt hit {target.name} | Chains left: {chainsRemaining}");

        // Apply damage (example)
        // var enemy = target.GetComponent<Enemy>();
        // if (enemy != null) enemy.TakeDamage(damage);

        if (chainsRemaining > 0)
        {
            Transform next = FindNextTarget();
            if (next != null)
            {
                Debug.Log($"🔗 Chaining to {next.name}");
                GameObject nextBolt = Instantiate(gameObject, target.position, Quaternion.identity);
                nextBolt.GetComponent<Bolt>().Initialize(next, damage, chainsRemaining - 1, chainRange, hitEnemies);
            }
            else
            {
                Debug.Log("❌ No valid next target found.");
            }
        }

        Destroy(gameObject);
    }

    Transform FindNextTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(target.position, chainRange);
        Transform closest = null;
        float closestDist = Mathf.Infinity;

        foreach (Collider2D h in hits)
        {
            if (h.CompareTag("Enemy") && !hitEnemies.Contains(h.transform))
            {
                float dist = Vector2.Distance(target.position, h.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = h.transform;
                }
            }
        }

        if (closest != null)
            Debug.Log($"✅ Found next target: {closest.name}");
        else
            Debug.Log("❌ No valid next target in range.");

        return closest;
    }

    void OnDrawGizmosSelected()
    {
        if (target != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(target.position, chainRange);
        }
    }
}
