using UnityEngine;
using System.Collections.Generic;

public class LightningChain : MonoBehaviour
{
    public GameObject lightningPrefab; // prefab for the bolt effect
    public float fireRate = 1f;
    public int level = 1;
    public int maxLevel = 5;

    public int maxChains = 2;      // how many extra enemies it can hit
    public float chainRange = 3f;  // how far it can jump between enemies
    public float range = 10f;      // how far from player the first target can be

    private float nextFireTime = 0f;
    private float damage;

    public LevelUpButtons levelUpButton;

    void Start()
    {
        SaveFile.Data loadedData = SaveFile.LoadData<SaveFile.Data>();
        damage = loadedData.currentDamage;

        levelUpButton.LevelUp(level, maxLevel);
    }

    void Update()
    {
        if (level <= 0) return; // Do nothing if un-leveled

        if (Time.time >= nextFireTime)
        {
            FireLightning();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void FireLightning()
    {
        Transform firstTarget = FindClosestEnemy(range);
        if (firstTarget == null) return;

        GameObject bolt = Instantiate(lightningPrefab, transform.position, Quaternion.identity);

        Bolt boltScript = bolt.GetComponent<Bolt>();
        if (boltScript != null)
        {
            boltScript.Initialize(firstTarget, damage, maxChains, chainRange);
        }
    }


    Transform FindClosestEnemy(float searchRange, Transform exclude = null)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, searchRange);
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy") && hit.transform != exclude)
            {
                float dist = Vector2.Distance(transform.position, hit.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = hit.transform;
                }
            }
        }

        return closest;
    }

    public void LevelUp()
    {
        level++;
        if (level > maxLevel) level = maxLevel;

        levelUpButton.LevelUp(level, maxLevel);

        // scale effect
        maxChains++;
        fireRate += 0.2f;
    }
}
