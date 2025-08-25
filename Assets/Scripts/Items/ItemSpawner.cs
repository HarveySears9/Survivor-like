using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    private GameObject[] dropItems; // Array of item prefabs
    private int[] dropWeights;      // Array of weights for items (should match dropItems length)
    public float dropChance = 0.5f; // Chance to drop an item

    public GameObject[] tier1, tier2, tier3;
    public int[] weights1, weights2, weights3;


    public GameObject[] bossDrops;

    void Start()
    {
        dropItems = tier1;
        dropWeights = weights1;

        GameTimer gameTimer = FindObjectOfType<GameTimer>();
        gameTimer.OnDropChange += UpdateDropTier;
    }

    void UpdateDropTier(int newTier)
    {
        GameObject[] newItems = null;
        int[] newWeights = null;

        switch (newTier)
        {
            case 1:
                newItems = tier1;
                newWeights = weights1;
                break;
            case 2:
                newItems = tier2;
                newWeights = weights2;
                break;
            case 3:
                newItems = tier3;
                newWeights = weights3;
                break;
        }

        // Only update if the arrays are not null and not empty
        if (newItems != null && newItems.Length > 0 &&
            newWeights != null && newWeights.Length > 0)
        {
            dropItems = newItems;
            dropWeights = newWeights;
        }
        else
        {
            Debug.LogWarning($"Drop tier {newTier} is empty! Staying on previous tier.");
        }
    }


    void OnEnable()
    {
        // Subscribe to the EnemyDeath event
        EnemyDeathEventManager.OnEnemyDeath += SpawnItem;

        EnemyDeathEventManager.OnBossDeath += SpawnBossItem;
    }

    void OnDisable()
    {
        // Unsubscribe from the event
        EnemyDeathEventManager.OnEnemyDeath -= SpawnItem;

        EnemyDeathEventManager.OnBossDeath -= SpawnBossItem;
    }

    void SpawnItem(Vector3 position)
    {
        // Check if an item should be dropped
        if (Random.value <= dropChance)
        {
            // Choose a random item based on weights
            int selectedIndex = GetWeightedRandomIndex(dropWeights);

            // Instantiate the selected item at the enemy's death position
            if (selectedIndex >= 0 && selectedIndex < dropItems.Length)
            {
                Instantiate(dropItems[selectedIndex], position, Quaternion.identity);
            }
        }
    }

    void SpawnBossItem(Vector3 position, GameObject[] dropPrefabs)
    {

        foreach (GameObject dropPrefab in dropPrefabs)
        {
            // Calculate a random offset within the specified variance
            Vector3 randomOffset = new Vector3(
                Random.Range(-0.25f, 0.25f),
                Random.Range(-0.15f, 0.15f),
                0f
            );

            // Instantiate the prefab at the position with the random offset
            Instantiate(dropPrefab, position + randomOffset, Quaternion.identity);
        }
    }

    int GetWeightedRandomIndex(int[] weights)
    {
        // Calculate the total weight
        int totalWeight = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            totalWeight += weights[i];
        }

        // Generate a random value between 0 and totalWeight
        int randomValue = Random.Range(0, totalWeight);

        // Determine which weight range the random value falls into
        int cumulativeWeight = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue < cumulativeWeight)
            {
                return i; // Return the index of the selected item
            }
        }

        return -1; // Return -1 if no index was selected (shouldn't happen if weights are valid)
    }
}
