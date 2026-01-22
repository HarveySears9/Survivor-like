using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStats : MonoBehaviour
{
    public EnemySpawner levelSpawner;

    public float spawnInterval = 0.75f;

    public float difficultyInterval = 90f; // Time interval for difficulty increase (in seconds)
    public float dropInterval = 90f;       // Time interval for drop tier increase (in seconds)
    public float bossInterval = 180f;      // Time interval for boss spawn (in seconds)
    public float waveInterval = 45f;       // time between waves
    public float structureInterval = 120f; // time between structures spawning

    // Start is called before the first frame update
    void Start()
    {
        levelSpawner.spawnInterval = spawnInterval;
        GameTimer gameTimer = FindObjectOfType<GameTimer>();

        gameTimer.SetIntervals(difficultyInterval, dropInterval, bossInterval, waveInterval, structureInterval);
    }
}
