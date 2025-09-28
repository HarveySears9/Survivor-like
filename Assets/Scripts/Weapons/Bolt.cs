using UnityEngine;

public class Bolt : MonoBehaviour
{
    public GameObject gfx;

    private Transform target;
    private float damage;
    private int chainsRemaining;
    private float chainRange;

    public float speed = 7f; // bolt travel speed

    public void Initialize(Transform firstTarget, float dmg, int chains, float range)
    {
        target = firstTarget;
        damage = dmg;
        chainsRemaining = chains;
        chainRange = range;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Calculate direction to target
        Vector2 direction = (target.position - transform.position).normalized;

        // Rotate gfx to face the movement direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gfx.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Move toward target
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // If we reached the target
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        if (chainsRemaining > 0)
        {
            Transform nextTarget = FindNextTarget();
            if (nextTarget != null)
            {
                GameObject nextBolt = Instantiate(gameObject, transform.position, Quaternion.identity);
                nextBolt.GetComponent<Bolt>()
                        .Initialize(nextTarget, damage, chainsRemaining - 1, chainRange);
            }
        }

        Destroy(gameObject); // remove this bolt once it hits
    }

    Transform FindNextTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, chainRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy") && hit.transform != target)
                return hit.transform;
        }
        return null;
    }
}
