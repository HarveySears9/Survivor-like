using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArcBolt : WeaponBase
{
    [Header("Arc Bolt")]
    public float baseRange = 5f;
    public float chainRange = 3f;
    public int maxChains = 3;

    [Header("Visuals")]
    public GameObject arcVisualPrefab;

    [Header("Timing")]
    public float chainDelay = 0.08f;

    [Header("Audio")]
    public AudioClip startSound;

    [Header("Level Up")]
    public LevelUpButtons levelUpButton;

    private List<Transform> hitEnemies =
        new List<Transform>();

    private bool isFiring = false;

    public bool testing = false;

    protected override void Start()
    {
        base.Start();

        level = 0;

        if (levelUpButton != null)
        {
            levelUpButton.LevelUp(level, maxLevel);
        }
    }

    void Update()
    {
        if (level <= 0)
            return;

        if (!isFiring && CooldownReady())
        {
            hitEnemies.Clear();

            Transform firstTarget =
                FindClosestEnemy(
                    transform.position,
                    baseRange
                );

            if (firstTarget != null)
            {
                StartCoroutine(
                    FireRoutine(firstTarget)
                );
            }
        }

        UpdateCooldownUI();
    }

    IEnumerator FireRoutine(Transform firstTarget)
    {
        if (firstTarget == null)
            yield break;

        isFiring = true;

        yield return StartCoroutine(
            ChainToTargetRoutine(
                firstTarget,
                maxChains,
                transform.position
            )
        );

        // Cooldown starts after the entire chain finishes
        StartCooldown();

        isFiring = false;
    }

    IEnumerator ChainToTargetRoutine(
        Transform target,
        int chainsRemaining,
        Vector3 startPos
    )
    {
        float finalDamage =
            GetWeaponDamage();

        while (
            target != null &&
            chainsRemaining > 0
        )
        {
            if (target == null)
                yield break;

            hitEnemies.Add(target);

            // Damage enemy
            EnemyController enemy =
                target.GetComponent<EnemyController>();

            // Damage boss
            Boss boss =
                target.GetComponent<Boss>();

            if (enemy != null)
            {
                enemy.TakeDamage(finalDamage);
            }

            if (boss != null)
            {
                boss.TakeDamage(finalDamage);
            }

            Vector3 targetPos =
                target.position;

            // Spawn lightning visual
            if (arcVisualPrefab != null)
            {
                SpawnLightningArc(
                    startPos,
                    targetPos
                );
            }

            yield return new WaitForSeconds(
                chainDelay
            );

            // Find the next enemy
            Transform nextTarget =
                FindClosestEnemy(
                    targetPos,
                    chainRange
                );

            if (
                nextTarget == null ||
                hitEnemies.Contains(nextTarget)
            )
            {
                break;
            }

            startPos = targetPos;
            target = nextTarget;

            chainsRemaining--;
        }
    }

    Transform FindClosestEnemy(
        Vector3 position,
        float range
    )
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                position,
                range
            );

        Transform closest = null;

        float closestDistance =
            Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            if (hit == null)
                continue;

            if (!hit.CompareTag("Enemy"))
                continue;

            Transform target =
                hit.transform;

            if (target == null)
                continue;

            if (hitEnemies.Contains(target))
                continue;

            float distance =
                Vector2.Distance(
                    position,
                    target.position
                );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = target;
            }
        }

        return closest;
    }

    void SpawnLightningArc(
        Vector3 start,
        Vector3 end
    )
    {
        if (arcVisualPrefab == null)
            return;

        GameObject visual =
            Instantiate(
                arcVisualPrefab,
                start,
                Quaternion.identity
            );

        Vector3 direction =
            end - start;

        float distance =
            direction.magnitude;

        if (distance > 0f)
        {
            visual.transform.right =
                direction.normalized;
        }

        SpriteRenderer spriteRenderer =
            arcVisualPrefab.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null &&
            spriteRenderer.sprite != null)
        {
            Vector3 scale =
                visual.transform.localScale;

            scale.x =
                distance /
                spriteRenderer.sprite.bounds.size.x;

            visual.transform.localScale =
                scale;
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(
                startSound
            );
        }
    }

    public override void LevelUp()
    {
        level++;

        if (level > maxLevel)
            level = maxLevel;

        if (levelUpButton != null)
        {
            levelUpButton.LevelUp(
                level,
                maxLevel
            );
        }

        if (level == 1)
        {
            CreateWeaponUI();
        }

        switch (level)
        {
            case 1:
                maxChains = 2;
                break;

            case 2:
                maxChains = 3;
                break;

            case 3:
                maxChains = 4;
                break;

            case 4:
                maxChains = 5;
                break;

            case 5:
                maxChains = 6;
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            baseRange
        );

        Gizmos.DrawWireSphere(
            transform.position,
            chainRange
        );
    }
}