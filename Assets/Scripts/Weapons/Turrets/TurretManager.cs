using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretManager : MonoBehaviour
{
    [Header("Turret Prefab Settings")]
    public GameObject turretPrefab;       // Prefab for the turret type
    public int level = 0;                 // Manager level, 0 = no turrets
    public int maxLevel = 5;              // Maximum upgrade level

    [Header("Turret Count Settings")]
    public int currentTurretCount = 0;    // Turrets currently in the scene
    public int maxTurretCount = 0;        // Maximum turrets allowed at current level
    public int maxUpgradeTurrets = 3;     // Hard cap on turrets at max level
    public List<TurretBase> activeTurrets = new List<TurretBase>();

    [Header("Spawn Settings")]
    public float spawnRadius = 2f;        // Distance from player to spawn turret
    public float respawnDelay = 0.5f;     // Delay before respawning after a turret expires

    private Transform playerTransform;
    private bool isRespawning = false;

    public LevelUpButtons levelUpButton;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        levelUpButton.LevelUp(level, maxLevel);
        TrySpawnTurret();
    }

    void Update()
    {
        // Remove destroyed turrets from the list
        for (int i = activeTurrets.Count - 1; i >= 0; i--)
        {
            if (activeTurrets[i] == null)
            {
                activeTurrets.RemoveAt(i);
                currentTurretCount--;
            }
        }

        // Auto-spawn if under maxTurretCount
        if (currentTurretCount < maxTurretCount && !isRespawning)
        {
            StartCoroutine(RespawnTurretAfterDelay());
        }
    }

    void TrySpawnTurret()
    {
        if (level <= 0) return;                    // Don't spawn at level 0
        if (currentTurretCount >= maxTurretCount) return;
        if (turretPrefab == null || playerTransform == null) return;

        Vector2 spawnPos = playerTransform.position + (Vector3)(Random.insideUnitCircle.normalized * spawnRadius);

        GameObject turretObj = Instantiate(turretPrefab, spawnPos, Quaternion.identity);
        TurretBase turret = turretObj.GetComponent<TurretBase>();
        if (turret != null)
        {
            activeTurrets.Add(turret);
            currentTurretCount++;

            // Apply upgrades for its level
            for (int i = 1; i < level; i++)
            {
                turret.LevelUp();
            }
        }
    }

    public void LevelUp()
    {
        if (level >= maxLevel) return;

        level++;

        // Increase max turrets for this level (cannot exceed hard cap)
        maxTurretCount = Mathf.Min(maxTurretCount + 1, maxUpgradeTurrets);

        // Level up all active turrets
        foreach (var turret in activeTurrets)
        {
            if (turret != null)
                turret.LevelUp();
        }

        levelUpButton.LevelUp(level, maxLevel);

        // Spawn new turret if under current maxTurretCount
        TrySpawnTurret();
    }

    private IEnumerator RespawnTurretAfterDelay()
    {
        isRespawning = true;
        yield return new WaitForSeconds(respawnDelay);
        isRespawning = false;
        TrySpawnTurret();
    }
}
