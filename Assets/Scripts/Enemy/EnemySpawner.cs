using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private GameObject[] enemyPrefabs;            // Reference to the enemy prefab
    private int[] spawnWeights;
    public float spawnInterval = 2f;          // Time between spawns
    public float circleRadius = 15f;          // Radius of the circle (larger than camera view)

    private float timer;
    private Transform playerTransform;        // Reference to the player's transform
    private Camera mainCamera;

    public GameObject[] tier1, tier2, tier3, tier4, tier5, tier6;
    public int[] weights1, weights2, weights3, weights4, weights5, weights6;

    public bool spawning;

    void Start()
    {
        // Find the player's transform and main camera at the start
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        mainCamera = Camera.main;  // Get the main camera

        GameTimer gameTimer = FindObjectOfType<GameTimer>();
        gameTimer.OnDifficultyChange += UpdateEnemyStats;

        UpdateEnemyStats(1);
    }

    void UpdateEnemyStats(int newTier)
    {
        GameObject[] newPrefabs = null;
        int[] newWeights = null;

        switch (newTier)
        {
            case 1:
                newPrefabs = tier1;
                newWeights = weights1;
                break;
            case 2:
                newPrefabs = tier2;
                newWeights = weights2;
                break;
            case 3:
                newPrefabs = tier3;
                newWeights = weights3;
                break;
            case 4:
                newPrefabs = tier4;
                newWeights = weights4;
                break;
                // case 5:
                //     newPrefabs = tier5;
                //     newWeights = weights5;
                //     break;
        }

        // Only update if the arrays are not null and not empty
        if (newPrefabs != null && newPrefabs.Length > 0 &&
            newWeights != null && newWeights.Length > 0)
        {
            enemyPrefabs = newPrefabs;
            spawnWeights = newWeights;
        }
        else
        {
            Debug.LogWarning($"Tier {newTier} is empty! Staying on previous tier.");
        }
    }


    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && spawning)
        {
            SpawnEnemy();
            timer = 0f;  // Reset the timer
        }
    }

    void SpawnEnemy()
    {
        // Randomly pick an angle in radians (0 to 360 degrees)
        float randomAngle = Random.Range(0f, 360f);

        // Calculate the spawn position using trigonometry
        float spawnX = playerTransform.position.x + circleRadius * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        float spawnY = playerTransform.position.y + circleRadius * Mathf.Sin(randomAngle * Mathf.Deg2Rad);

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f); // Position to spawn the enemy

        // Determine which enemy to spawn using weighted random selection
        GameObject selectedEnemy = GetRandomWeightedEnemy();

        // Instantiate the selected enemy at the calculated position
        GameObject enemyObj = Instantiate(selectedEnemy, spawnPosition, Quaternion.identity);

        // Get the EnemyController component from the instantiated enemy
        EnemyController enemy = enemyObj.GetComponent<EnemyController>();

        // Assign the player's transform to the enemy to target the player
        enemy.playerTransform = playerTransform;
    }

    GameObject GetRandomWeightedEnemy()
    {
        // Calculate the total weight
        int totalWeight = 0;
        for (int i = 0; i < spawnWeights.Length; i++)
        {
            totalWeight += spawnWeights[i];
        }

        // Generate a random value within the range of totalWeight
        int randomWeight = Random.Range(0, totalWeight);

        // Iterate through the weights to determine which prefab to select
        int cumulativeWeight = 0;
        for (int i = 0; i < spawnWeights.Length; i++)
        {
            cumulativeWeight += spawnWeights[i];
            if (randomWeight < cumulativeWeight)
            {
                return enemyPrefabs[i];
            }
        }

        // Fallback in case no selection is made (shouldn't happen)
        return enemyPrefabs[0];
    }
}