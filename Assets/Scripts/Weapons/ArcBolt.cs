using UnityEngine;
using System.Collections.Generic;

public class ArcBolt : MonoBehaviour
{
    [Header("Attack Settings")]
    public float fireRate = 1f;
    public float baseRange = 5f;   // range to find the first enemy
    public float chainRange = 3f;  // distance for chaining
    public float damage = 1f;
    public int maxChains = 3;

    [Header("Level Settings")]
    public int level = 1;
    public int maxLevel = 5;
    public LevelUpButtons levelUpButton;

    [Header("Visuals")]
    public GameObject arcVisualPrefab; // simple graphic (line or particle)

    private float nextFireTime = 0f;
    private List<Transform> hitEnemies = new List<Transform>();

    void Start()
    {
        if (levelUpButton != null)
            levelUpButton.LevelUp(level, maxLevel);
    }

    void Update()
    {
        if (level <= 0) return;

        if (Time.time >= nextFireTime)
        {
            Transform firstTarget = FindClosestEnemy(transform.position, baseRange);
            if (firstTarget != null)
                Fire(firstTarget);

            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    /// <summary>
    /// Fires the ArcBolt starting from the first enemy
    /// </summary>
    public void Fire(Transform firstTarget)
    {
        if (firstTarget == null) return;

        hitEnemies.Clear();
        // Pass player's position as start of first arc
        ChainToTarget(firstTarget, maxChains, transform.position);
    }

    /// <summary>
    /// Chains to target recursively
    /// </summary>
    void ChainToTarget(Transform target, int chainsRemaining, Vector3 startPos)
    {
        if (target == null || chainsRemaining <= 0) return;

        hitEnemies.Add(target);

        // Deal damage to the enemy
        var enemy = target.GetComponent<EnemyController>();
        var boss = target.GetComponent<Boss>();

        if (enemy != null) enemy.TakeDamage(damage);
        if (boss != null) boss.TakeDamage(damage);


        // Show visual prefab if assigned
        if (arcVisualPrefab != null)
        {
            GameObject visual = Instantiate(arcVisualPrefab, target.position, Quaternion.identity);
            Destroy(visual, 0.2f);
        }

        // Draw line from startPos to target
        DrawLine(startPos, target.position);

        // Find next target
        Transform nextTarget = FindClosestEnemy(target.position, chainRange);
        if (nextTarget != null && !hitEnemies.Contains(nextTarget))
        {
            // Recursive chain: target becomes new start
            ChainToTarget(nextTarget, chainsRemaining - 1, target.position);
        }
    }

    /// <summary>
    /// Finds the closest enemy to a position within a given range
    /// </summary>
    Transform FindClosestEnemy(Vector3 position, float range)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, range);
        Transform closest = null;
        float closestDist = Mathf.Infinity;

        foreach (var h in hits)
        {
            if (h.CompareTag("Enemy") && !hitEnemies.Contains(h.transform))
            {
                float dist = Vector2.Distance(position, h.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = h.transform;
                }
            }
        }

        return closest;
    }

    /// <summary>
    /// Draws a temporary line between two positions on the Projectile sorting layer
    /// </summary>
    void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("ArcLine");
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.yellow;
        lr.endColor = Color.yellow;

        // Set sorting layer for visuals
        lr.sortingLayerName = "Projectile";
        lr.sortingOrder = 5;

        Destroy(lineObj, 0.2f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, baseRange);
    }

    /// <summary>
    /// Level up the ArcBolt, increasing max chains
    /// </summary>
    public void LevelUp()
    {
        level++;
        if (level > maxLevel) level = maxLevel;

        if (levelUpButton != null)
            levelUpButton.LevelUp(level, maxLevel);

        switch (level)
        {
            case 1: maxChains = 2; break;
            case 2: maxChains = 3; break;
            case 3: maxChains = 4; break;
            case 4: maxChains = 5; break;
            case 5: maxChains = 6; break;
        }
    }
}
