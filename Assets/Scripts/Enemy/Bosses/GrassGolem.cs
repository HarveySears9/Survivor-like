using System.Collections;
using UnityEngine;

public class GrassGolem : Boss
{
    public GameObject sporeCloudPrefab; // Reference to the spore cloud prefab
    public float sporeCooldown = 5f;   // Time between spore cloud attacks
    public GameObject minionPrefab;   // Reference to the minion prefab
    public float minionSpawnCooldown = 8f; // Time between minion spawns

    private float nextSporeTime;
    private float nextMinionSpawnTime;

    protected override void Start()
    {
        base.Start();
        // Initialize additional Grass Golem-specific logic
        nextSporeTime = Time.time + sporeCooldown;
        nextMinionSpawnTime = Time.time + minionSpawnCooldown;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        PerformGrassGolemAbilities();
    }

    private void PerformGrassGolemAbilities()
    {
        // Spore Cloud Attack
        if (Time.time >= nextSporeTime)
        {
            //StartCoroutine(ReleaseSporeCloud());
            nextSporeTime = Time.time + sporeCooldown;
        }

        // Summon Minions
        if (Time.time >= nextMinionSpawnTime)
        {
            StartCoroutine(SpawnMinions());
            nextMinionSpawnTime = Time.time + minionSpawnCooldown;
        }
    }

    private IEnumerator ReleaseSporeCloud()
    {
        moving = false; // Stop movement while performing the ability

        yield return new WaitForSeconds(0.25f); // Optional delay before spawning the cloud

        if (sporeCloudPrefab != null)
        {
            // Instantiate the spore cloud at the Grass Golem's position
            Instantiate(sporeCloudPrefab, transform.position, Quaternion.identity);
            Debug.Log($"{Name} released a spore cloud!");
        }

        yield return new WaitForSeconds(0.5f); // Optional delay after the ability before resuming movement
        moving = true;
    }

    private IEnumerator SpawnMinions()
    {
        moving = false; // Stop movement while performing the ability

        yield return new WaitForSeconds(0.25f); // Optional delay before spawning minions

        if (minionPrefab != null)
        {
            // Spawn minions at random positions near the Grass Golem
            for (int i = 0; i < 3; i++) // Spawns 3 minions
            {
                Vector3 spawnPosition = transform.position + new Vector3(
                    Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f); // Slightly randomized spawn positions
                GameObject minionObj = Instantiate(minionPrefab, spawnPosition, Quaternion.identity);

                // Get the EnemyController component from the instantiated enemy
                EnemyController minion = minionObj.GetComponent<EnemyController>();

                // Assign the player's transform to the enemy to target the player
                minion.playerTransform = playerTransform; ;
                yield return new WaitForSeconds(0.1f); // Slight delay between each minion spawn
            }
            Debug.Log($"{Name} summoned minions!");
        }

        yield return new WaitForSeconds(0.25f); // Optional delay after the ability before resuming movement
        moving = true;
    }
}
