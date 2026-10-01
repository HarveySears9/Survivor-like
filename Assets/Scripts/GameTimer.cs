using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float elapsedTime = 0f;
    public TextMeshProUGUI timerText;

    [Header("Difficulty")]
    public int difficulty = 1;
    public int dropTier = 1;
    public int bossCount = 0;

    private float difficultyInterval;
    private float dropInterval;
    private float bossInterval;

    private float nextDifficultyTime;
    private float nextDropTime;
    private float nextBossTime;

    private bool isPaused = true;

    [Header("Wave")]
    private float waveInterval;
    private float nextWaveTime;

    [Header("Structure")]
    private float structureInterval;
    private float nextStructureTime;

    [Header("Spawn Pacing")]
    public EnemySpawner enemySpawner;

    public float earlySpawnInterval = 1.2f;
    public float midSpawnInterval = 0.9f;
    public float normalSpawnInterval = 0.6f;

    public delegate void DifficultyChange(int newDifficulty);
    public event DifficultyChange OnDifficultyChange;

    public delegate void DropChange(int newDropTier);
    public event DropChange OnDropChange;

    public delegate void SpawnBoss(int newBossNumber);
    public event SpawnBoss OnSpawnBoss;

    public delegate void WaveStart();
    public event WaveStart OnWaveStart;

    public delegate void SpawnStructure();
    public event SpawnStructure OnSpawnStructure;

    public void SetIntervals(
        float diff,
        float drop,
        float boss,
        float wave,
        float structure)
    {
        difficultyInterval = diff;
        dropInterval = drop;
        bossInterval = boss;
        waveInterval = wave;
        structureInterval = structure;

        nextDifficultyTime = difficultyInterval;
        nextDropTime = dropInterval;
        nextBossTime = bossInterval;
        nextWaveTime = waveInterval;
        nextStructureTime = structureInterval;

        isPaused = false;
    }

    void Start()
    {
        isPaused = true;
        UpdateTimerUI();
    }

    void Update()
    {
        if (isPaused)
            return;

        elapsedTime += Time.deltaTime;

        UpdateSpawnInterval();

        if (elapsedTime >= nextDifficultyTime)
        {
            difficulty++;
            nextDifficultyTime += difficultyInterval;

            OnDifficultyChange?.Invoke(difficulty);
        }

        if (elapsedTime >= nextDropTime)
        {
            dropTier++;
            nextDropTime += dropInterval;

            OnDropChange?.Invoke(dropTier);
        }

        if (elapsedTime >= nextBossTime)
        {
            bossCount++;
            nextBossTime += bossInterval;

            OnSpawnBoss?.Invoke(bossCount);
        }

        if (elapsedTime >= nextWaveTime)
        {
            nextWaveTime += waveInterval;

            OnWaveStart?.Invoke();
        }

        if (elapsedTime >= nextStructureTime)
        {
            nextStructureTime += structureInterval;

            OnSpawnStructure?.Invoke();
        }

        UpdateTimerUI();
    }

    void UpdateSpawnInterval()
    {
        if (enemySpawner == null)
            return;

        if (elapsedTime < 30f)
        {
            enemySpawner.baseSpawnInterval = earlySpawnInterval;
        }
        else if (elapsedTime < 60f)
        {
            enemySpawner.baseSpawnInterval = midSpawnInterval;
        }
        else
        {
            enemySpawner.baseSpawnInterval = normalSpawnInterval;
        }

        // Only update the actual spawn interval if a wave
        // isn't currently overriding it.
        if (!enemySpawner.waveActive)
        {
            enemySpawner.spawnInterval =
                enemySpawner.baseSpawnInterval;
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);

            timerText.text = $"{minutes:D2}:{seconds:D2}";
        }
    }

    public void PauseTimer()
    {
        isPaused = true;
    }

    public void ResumeTimer()
    {
        isPaused = false;
    }
}