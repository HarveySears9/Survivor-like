using UnityEngine;
using System.Collections;
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

    [Header("Timing Settings")]
    public float chainDelay = 0.08f; // delay between each lightning jump

    private float nextFireTime = 0f;
    private List<Transform> hitEnemies = new List<Transform>();
    private bool isFiring = false;


    public bool testing = false;

    private PlayerController player;

    void Start()
    {
        if (levelUpButton != null)
            levelUpButton.LevelUp(level, maxLevel);

        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (level <= 0 || isFiring) return;

        if (Time.time >= nextFireTime)
        {
            hitEnemies.Clear();
            Transform firstTarget = FindClosestEnemy(transform.position, baseRange);
            if (firstTarget != null)
            {
                StartCoroutine(FireRoutine(firstTarget));
            }
        }
    }

    IEnumerator FireRoutine(Transform firstTarget)
    {
        if (firstTarget == null) yield break;

        isFiring = true;
        yield return StartCoroutine(ChainToTargetRoutine(firstTarget, maxChains, transform.position));
        
        float effectiveFireRate = fireRate;
        effectiveFireRate *= player.attackSpeedMultiplier;
        nextFireTime = Time.time + 1f / effectiveFireRate;
        
        isFiring = false;
    }

    IEnumerator ChainToTargetRoutine(Transform target, int chainsRemaining, Vector3 startPos)
    {

        while (target != null && chainsRemaining > 0)
        {
            // if target got destroyed, stop immediately
            if (target == null)
                yield break;

            hitEnemies.Add(target);

            // Damage enemy safely
            var enemy = target.GetComponent<EnemyController>();
            var boss = target.GetComponent<Boss>();

            if (enemy != null) enemy.TakeDamage(damage);
            if (boss != null) boss.TakeDamage(damage);

            Vector3 targetPos = target != null ? target.position : startPos;

            // Visual
            if (arcVisualPrefab != null)
                SpawnLightningArc(startPos, targetPos);


            // Line
            // DrawLine(startPos, targetPos);

            yield return new WaitForSeconds(chainDelay);

            // Find next target
            Transform nextTarget = FindClosestEnemy(targetPos, chainRange);

            // If next target is invalid, break
            if (nextTarget == null || hitEnemies.Contains(nextTarget))
                break;

            // Prep for next chain
            startPos = targetPos;
            target = nextTarget;
            chainsRemaining--;
        }
    }

    Transform FindClosestEnemy(Vector3 position, float range)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, range);

        Debug.Log($"Found {hits.Length} colliders in range {range}");
        foreach (var h in hits)
        {
            Debug.Log($"{h.name} - Tag: {h.tag}");
        }

        Transform closest = null;
        float closestDist = Mathf.Infinity;

        foreach (var h in hits)
        {
            if (h == null) continue; // skip destroyed colliders
            if (h.CompareTag("Enemy") && !hitEnemies.Contains(h.transform))
            {
                if (h.transform == null) continue; // safeguard
                float dist = Vector2.Distance(position, h.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = h.transform;
                }
            }
        }

        Debug.Log($"{closest}");
        return closest;
    }

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

        lr.sortingLayerName = "Projectile";
        lr.sortingOrder = 5;

        Destroy(lineObj, 0.2f);
    }

    void SpawnLightningArc(Vector3 start, Vector3 end)
    {
        if (arcVisualPrefab == null) return;

        GameObject visual = Instantiate(arcVisualPrefab, start, Quaternion.identity);

        // Compute the direction and distance
        Vector3 dir = end - start;
        float distance = dir.magnitude;

        // Rotate the prefab to face the target
        visual.transform.right = dir.normalized;

        // Scale along X to stretch between points
        Vector3 scale = visual.transform.localScale;
        scale.x = distance / (arcVisualPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x); // Adjust for sprite width
        visual.transform.localScale = scale;
    }

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


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, baseRange);
        Gizmos.DrawWireSphere(transform.position, chainRange);
    }
}