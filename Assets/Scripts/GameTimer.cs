using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float elapsedTime = 0f;           // Timer to track total time elapsed
    public TextMeshProUGUI timerText;       // Reference to the TextMeshProUGUI component
    public int difficulty = 1;             // Current difficulty level
    public int dropTier = 1;               // Current drop tier
    public int bossCount = 0;              // Number of bosses spawned
    public float difficultyInterval = 30f; // Time interval for difficulty increase (in seconds)
    public float dropInterval = 60f;       // Time interval for drop tier increase (in seconds)
    public float bossInterval = 120f;      // Time interval for boss spawn (in seconds)

    private float nextDifficultyTime;
    private float nextDropTime;
    private float nextBossTime;
    private bool isPaused = false;         // Flag to pause the timer

    public delegate void DifficultyChange(int newDifficulty);
    public event DifficultyChange OnDifficultyChange;

    public delegate void DropChange(int newDropTier);
    public event DropChange OnDropChange;

    public delegate void SpawnBoss(int newBossNumber);
    public event SpawnBoss OnSpawnBoss;

    public delegate void WaveStart();
    public event WaveStart OnWaveStart;
    public float waveInterval = 30f; // time between waves

    private float nextWaveTime;


    void Start()
    {
        nextDifficultyTime = difficultyInterval;
        nextDropTime = dropInterval;
        nextBossTime = bossInterval;
        nextWaveTime = waveInterval;

        UpdateTimerUI();  // Initialize timer display
    }

    void Update()
    {
        if (isPaused) return; // Exit if the timer is paused

        // Update elapsed time
        elapsedTime += Time.deltaTime;

        // Check if it's time to increase difficulty
        if (elapsedTime >= nextDifficultyTime)
        {
            difficulty++;
            nextDifficultyTime += difficultyInterval; // Schedule the next difficulty change
            OnDifficultyChange?.Invoke(difficulty);   // Trigger the event
        }

        // Check if it's time to increase drop tier
        if (elapsedTime >= nextDropTime)
        {
            dropTier++;
            nextDropTime += dropInterval; // Schedule the next drop change
            OnDropChange?.Invoke(dropTier); // Trigger the event
        }

        // Check if it's time to spawn a boss
        if (elapsedTime >= nextBossTime)
        {
            bossCount++;
            nextBossTime += bossInterval; // Schedule the next boss spawn
            OnSpawnBoss?.Invoke(bossCount); // Trigger the event
        }

        // Waves
        if (elapsedTime >= nextWaveTime)
        {
            nextWaveTime += waveInterval;
            OnWaveStart?.Invoke();
        }

        // Update the timer display
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            // Format elapsed time as MM:SS
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
