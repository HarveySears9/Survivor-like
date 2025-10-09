using UnityEngine;

public class Bolt : MonoBehaviour
{
    public GameObject gfx;

    private Transform target;
    private float damage;
    private int chainsRemaining;
    private float chainRange;

    public float speed = 7f; // bolt travel speed

    private bool hit = false;

    public void Initialize(Transform firstTarget, float dmg, int chains, float range)
    {
        target = firstTarget;
        damage = dmg;
        chainsRemaining = chains;
        chainRange = range;
    }

    void Update()
    {
        // If target got destroyed, try to find a new one
        if (target != null)
        {
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
        else
        {
            target = FindNextTarget();
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }
        }
    }

    void HitTarget()
    {
        if (hit) { return; }

        hit = true;

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
        Transform closest = null;
        float closestDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy") && hit.transform != target)
            {
                float dist = Vector2.Distance(transform.position, hit.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = hit.transform;
                }
            }
        }

        return closest;
    }
}
