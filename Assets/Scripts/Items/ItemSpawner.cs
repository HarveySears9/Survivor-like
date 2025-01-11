using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] dropItems; // Array of item prefabs
    public int[] dropWeights;      // Array of weights for items (should match dropItems length)
    public float dropChance = 0.5f; // Chance to drop an item

    void OnEnable()
    {
        // Subscribe to the EnemyDeath event
        EnemyDeathEventManager.OnEnemyDeath += SpawnItem;
    }

    void OnDisable()
    {
        // Unsubscribe from the event
        EnemyDeathEventManager.OnEnemyDeath -= SpawnItem;
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
