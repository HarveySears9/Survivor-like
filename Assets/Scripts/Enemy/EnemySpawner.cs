using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public float spawnInterval = 2f;   // Time between spawns
    public float circleRadius = 15f;   // Radius of spawn circle
    public bool spawning = false;      // Whether spawner is active

    [Header("Enemy Spawn Database")]
    public EnemySpawnsDatabase enemyDatabase;   // Reference to the ScriptableObject

    private GameObject[] enemyPrefabs;          // Current tier’s enemies
    private int[] spawnWeights;                 // Current tier’s weights

    private float timer;
    private Transform playerTransform;
    private Camera mainCamera;

    void Start()
    {
        // Get player + camera
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        mainCamera = Camera.main;

        if (playerTransform == null)
        {
            Debug.LogError("Player not found! Make sure it has the 'Player' tag.");
            return;
        }

        if (enemyDatabase == null || enemyDatabase.enemySpawns == null || enemyDatabase.enemySpawns.Length == 0)
        {
            Debug.LogError("EnemySpawnsDatabase not assigned or empty!");
            return;
        }

        // Subscribe to events
        GameTimer gameTimer = FindObjectOfType<GameTimer>();
        if (gameTimer != null)
        {
            gameTimer.OnDifficultyChange += UpdateEnemyStats;
            gameTimer.OnWaveStart += StartWave;
        }
        else
        {
            Debug.LogWarning("GameTimer not found — difficulty and wave changes won't trigger.");
        }

        // Initialize to first tier
        UpdateEnemyStats(1);
    }

    void UpdateEnemyStats(int newTier)
    {
        // Ensure tier is within valid range
        int tierIndex = Mathf.Clamp(newTier - 1, 0, enemyDatabase.enemySpawns.Length - 1);

        EnemySpawns data = enemyDatabase.enemySpawns[tierIndex];

        if (data.enemyPrefabs != null && data.enemyPrefabs.Length > 0 &&
            data.weights != null && data.weights.Length > 0)
        {
            enemyPrefabs = data.enemyPrefabs;
            spawnWeights = data.weights;
        }
        else
        {
            Debug.LogWarning($"Tier {newTier} in database is empty or invalid — keeping previous settings.");
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (spawning && timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void StartWave()
    {
        StartCoroutine(Wave());
    }

    IEnumerator Wave()
    {
        float originalInterval = spawnInterval;
        spawnInterval = 0.1f;
        yield return new WaitForSeconds(5f);
        spawnInterval = originalInterval;
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("No enemy prefabs assigned for this tier!");
            return;
        }

        // Random angle for spawn around player
        float randomAngle = Random.Range(0f, 360f);
        float spawnX = playerTransform.position.x + circleRadius * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        float spawnY = playerTransform.position.y + circleRadius * Mathf.Sin(randomAngle * Mathf.Deg2Rad);

        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0f);
        GameObject selectedEnemy = GetRandomWeightedEnemy();

        GameObject enemyObj = Instantiate(selectedEnemy, spawnPos, Quaternion.identity);

        // Assign target
        EnemyController enemy = enemyObj.GetComponent<EnemyController>();
        if (enemy != null)
            enemy.playerTransform = playerTransform;
    }

    GameObject GetRandomWeightedEnemy()
    {
        if (spawnWeights == null || spawnWeights.Length != enemyPrefabs.Length)
        {
            Debug.LogWarning("Spawn weights and prefabs mismatch! Defaulting to first enemy.");
            return enemyPrefabs[0];
        }

        int totalWeight = 0;
        foreach (int w in spawnWeights)
            totalWeight += w;

        int randomWeight = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        for (int i = 0; i < spawnWeights.Length; i++)
        {
            cumulativeWeight += spawnWeights[i];
            if (randomWeight < cumulativeWeight)
                return enemyPrefabs[i];
        }

        // Fallback
        return enemyPrefabs[0];
    }
}
