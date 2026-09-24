using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    private GameObject[] dropItems;
    private int[] dropWeights;

    public float dropChance = 0.5f;

    public GameObject[] tier1, tier2, tier3;
    public int[] weights1, weights2, weights3;

    public GameObject[] bossDrops;

    // Cache the total weight so we don't recalculate it for every death
    private int totalDropWeight;

    void Start()
    {
        dropItems = tier1;
        dropWeights = weights1;

        CalculateTotalDropWeight();

        GameTimer gameTimer = FindObjectOfType<GameTimer>();
        gameTimer.OnDropChange += UpdateDropTier;
    }

    void OnDestroy()
    {
        GameTimer gameTimer = FindObjectOfType<GameTimer>();

        if (gameTimer != null)
            gameTimer.OnDropChange -= UpdateDropTier;
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

        if (newItems != null && newItems.Length > 0 &&
            newWeights != null && newWeights.Length > 0)
        {
            dropItems = newItems;
            dropWeights = newWeights;

            CalculateTotalDropWeight();
        }
        else
        {
            Debug.LogWarning(
                $"Drop tier {newTier} is empty! Staying on previous tier."
            );
        }
    }

    void CalculateTotalDropWeight()
    {
        totalDropWeight = 0;

        if (dropWeights == null)
            return;

        for (int i = 0; i < dropWeights.Length; i++)
        {
            totalDropWeight += dropWeights[i];
        }
    }

    void OnEnable()
    {
        EnemyDeathEventManager.OnEnemyDeath += SpawnItem;
        EnemyDeathEventManager.OnBossDeath += SpawnBossItem;
    }

    void OnDisable()
    {
        EnemyDeathEventManager.OnEnemyDeath -= SpawnItem;
        EnemyDeathEventManager.OnBossDeath -= SpawnBossItem;
    }

    void SpawnItem(Vector3 position)
    {
        if (Random.value > dropChance)
            return;

        int selectedIndex = GetWeightedRandomIndex();

        if (selectedIndex >= 0 &&
            selectedIndex < dropItems.Length)
        {
            Instantiate(
                dropItems[selectedIndex],
                position,
                Quaternion.identity
            );
        }
    }

    void SpawnBossItem(Vector3 position, GameObject[] dropPrefabs)
    {
        foreach (GameObject dropPrefab in dropPrefabs)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-0.25f, 0.25f),
                Random.Range(-0.15f, 0.15f),
                0f
            );

            Instantiate(
                dropPrefab,
                position + randomOffset,
                Quaternion.identity
            );
        }
    }

    int GetWeightedRandomIndex()
    {
        if (totalDropWeight <= 0)
            return -1;

        int randomValue = Random.Range(0, totalDropWeight);

        int cumulativeWeight = 0;

        for (int i = 0; i < dropWeights.Length; i++)
        {
            cumulativeWeight += dropWeights[i];

            if (randomValue < cumulativeWeight)
                return i;
        }

        return -1;
    }
}