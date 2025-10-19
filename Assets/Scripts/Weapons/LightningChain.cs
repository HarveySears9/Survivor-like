using UnityEngine;

public class LightningChain : MonoBehaviour
{
    public GameObject lightningPrefab;
    public float fireRate = 1f;
    public int level = 1;
    public int maxLevel = 5;

    public int maxChains = 1;
    public float chainRange = 3f;
    public float range = 10f;

    private float nextFireTime = 0f;
    private float damage = 10f; // fallback for testing

    public LevelUpButtons levelUpButton;

    void Start()
    {
        // Optional save load
        // SaveFile.Data loadedData = SaveFile.LoadData<SaveFile.Data>();
        // damage = loadedData.currentDamage;

        if (levelUpButton != null)
            levelUpButton.LevelUp(level, maxLevel);
    }

    void Update()
    {
        if (level <= 0) return;

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

        Debug.Log($"⚡ Firing first bolt at {firstTarget.name}");

        GameObject bolt = Instantiate(lightningPrefab, transform.position, Quaternion.identity);
        bolt.GetComponent<Bolt>().Initialize(firstTarget, damage, maxChains, chainRange);
    }

    Transform FindClosestEnemy(float searchRange)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, searchRange);
        Transform closest = null;
        float minDist = Mathf.Infinity;

        foreach (var h in hits)
        {
            if (h.CompareTag("Enemy"))
            {
                float dist = Vector2.Distance(transform.position, h.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = h.transform;
                }
            }
        }

        return closest;
    }

    public void LevelUp()
    {
        level++;
        if (level > maxLevel) level = maxLevel;

        if (levelUpButton != null)
            levelUpButton.LevelUp(level, maxLevel);

        switch (level)
        {
            case 1:
                maxChains = 1;
                break;
            case 2:
                maxChains = 2;
                break;
            case 3:
                maxChains = 3;
                break;
            case 4:
                maxChains = 4;
                break;
            case 5:
                maxChains = 5;
                break;
        }

        Debug.Log($"⚡ Lightning upgraded! Level: {level}, Max Chains: {maxChains}");
    }
}
