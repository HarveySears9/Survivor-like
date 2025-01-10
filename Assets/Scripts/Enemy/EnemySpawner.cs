using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;            // Reference to the enemy prefab
    public float spawnInterval = 2f;          // Time between spawns
    public float circleRadius = 15f;          // Radius of the circle (larger than camera view)

    private float timer;
    private Transform playerTransform;        // Reference to the player's transform
    private Camera mainCamera;

    void Start()
    {
        // Find the player's transform and main camera at the start
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        mainCamera = Camera.main;  // Get the main camera
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
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

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);  // Position to spawn the enemy

        // Instantiate the enemy at the calculated position
        GameObject enemyObj = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Get the EnemyController component from the instantiated enemy
        EnemyController enemy = enemyObj.GetComponent<EnemyController>();

        // Assign the player's transform to the enemy to target the player
        enemy.playerTransform = playerTransform;
    }
}
