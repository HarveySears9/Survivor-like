using UnityEngine;
using System.Collections;

public class MeteorDrop : MonoBehaviour
{
    public GameObject meteorPrefab;
    public Transform player;

    public float fireRate = 5f;   // How often meteors drop
    public int level = 1;
    public int maxLevel = 5;

    public float radius = 5f;     // spawn radius around player
    public int meteorsPerWave = 0; // can scale with level
    public int maxMeteorsPerWave = 5;

    private float nextFireTime = 0f;
    private bool firing = false;

    public LevelUpButtons levelUpButton;

    private PlayerController playerController;

    void Start()
    {
        levelUpButton.LevelUp(level, maxLevel);
        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (!firing && Time.time >= nextFireTime)
        {
            StartCoroutine(SpawnMeteors());
            float effectiveFireRate = fireRate;
            effectiveFireRate *= playerController.attackSpeedMultiplier;
            nextFireTime = Time.time + 1f / effectiveFireRate;
        }
    }

    IEnumerator SpawnMeteors()
    {
        firing = true;

        for (int i = 0; i < meteorsPerWave; i++)
        {
            Vector2 targetPos = GetRandomPositionAroundPlayer();

            // Spawn meteor above target
            GameObject meteor = Instantiate(meteorPrefab, targetPos + Vector2.up * 10f, Quaternion.identity);

            // Assign target position for shadow
            Meteor meteorScript = meteor.GetComponent<Meteor>();
            if (meteorScript != null)
                meteorScript.targetPosition = targetPos;

            // Wait a small random amount before spawning the next meteor
            float randomDelay = Random.Range(0.1f, 0.5f); // adjust min/max delay as you like
            yield return new WaitForSeconds(randomDelay);
        }

        nextFireTime = Time.time + 1f / fireRate;

        firing = false;
    }

    Vector2 GetRandomPositionAroundPlayer()
    {
        Vector2 offset = Random.insideUnitCircle * radius;
        return (Vector2)player.position + offset;
    }

    public void LevelUp()
    {
        level++;
        if (level > maxLevel) level = maxLevel;

        levelUpButton.LevelUp(level, maxLevel);

        // increase meteors per wave with level
        meteorsPerWave++;
        if (meteorsPerWave > maxMeteorsPerWave)
            meteorsPerWave = maxMeteorsPerWave;
    }
}
