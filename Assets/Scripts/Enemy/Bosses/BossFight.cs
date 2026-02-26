using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossFight : MonoBehaviour
{
    public GameTimer gameTimer;
    public TextMeshProUGUI bossText;

    private Transform playerTransform;

    public float circleRadius = 15f;

    public GameObject[] bosses;

    public GameObject bossHpBar;

    public int delay;

    private EnemySpawner spawner;

    [SerializeField] private WorldMessageUI worldMessage;

    private int currentBossIndex = 0;

    public GameObject winScreen;


    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        gameTimer = FindObjectOfType<GameTimer>();
        gameTimer.OnSpawnBoss += OnSpawnBoss;
        spawner = GetComponent<EnemySpawner>();

        EnemyDeathEventManager.OnBossDeath += OnBossDeath;
    }

    public void OnSpawnBoss(int bossNumber)
    {
        if (gameTimer != null)
        {
            gameTimer.PauseTimer(); // Pause the timer when the boss spawns
            Debug.Log("Game Timer Paused for Boss Fight");
        }

        spawner.spawning = false;
        StartCoroutine(BossDelay());
    }

    private IEnumerator BossDelay()
    {
        yield return new WaitForSeconds(delay);
        SpawnBoss();
    }


    public void OnBossDeath(Vector3 position, GameObject[] dropPrefab)
    {
        if (gameTimer != null)
        {
            worldMessage.ShowMessage("BOSS DEFEATED");

            bossHpBar.SetActive(false);

            currentBossIndex++; // Move to next boss in order

            if (currentBossIndex >= bosses.Length)
            {
                Debug.Log("All bosses defeated! LEVEL COMPLETE!");
                StartCoroutine(LevelComplete());
            }
            else
            {
                gameTimer.ResumeTimer();
                spawner.spawning = true;
            }
        }
    }

    void SpawnBoss()
    {
        // Randomly pick an angle in radians (0 to 360 degrees)
        float randomAngle = Random.Range(0f, 360f);

        // Calculate the spawn position using trigonometry
        float spawnX = playerTransform.position.x + circleRadius * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        float spawnY = playerTransform.position.y + circleRadius * Mathf.Sin(randomAngle * Mathf.Deg2Rad);

        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f); // Position to spawn the enemy

        // Instantiate the selected enemy at the calculated position
        if (currentBossIndex >= bosses.Length)
        {
            Debug.Log("All bosses already spawned.");
            return;
        }

        GameObject enemyObj = Instantiate(bosses[currentBossIndex], spawnPosition, Quaternion.identity);


        // Get the EnemyController component from the instantiated enemy
        Boss boss = enemyObj.GetComponent<Boss>();

        // Assign the player's transform to the enemy to target the player
        boss.playerTransform = playerTransform;

        boss.healthBar = bossHpBar.GetComponent<HealthBar>();

        boss.healthBar.SetMaxHealth(boss.maxHP);

        bossHpBar.SetActive(true);

        bossText.text = boss.Name;
    }

    private IEnumerator LevelComplete()
    {
        Debug.Log("Opening Level Complete Menu");

        // Stop spawning
        spawner.spawning = false;

        // Pause timer
        gameTimer.PauseTimer();

        yield return new WaitForSeconds(delay);

        Time.timeScale = 0f;
        if (winScreen != null)
        {
            winScreen.SetActive(true);
        }
    }

}
