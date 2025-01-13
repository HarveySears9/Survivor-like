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

    public GameObject[] tier1, tier2, tier3;
    public int[] weights1, weights2, weights3;

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
        switch (newTier)
        {
            case 1:
                enemyPrefabs = tier1;
                spawnWeights = weights1;
                break;
            case 2:
                enemyPrefabs = tier2;
                spawnWeights = weights2;
                break;
            case 3:
                enemyPrefabs = tier3;
                spawnWeights = weights3;
                break;

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