using UnityEngine;

public class MeteorDrop : MonoBehaviour
{
    public GameObject meteorPrefab;
    public Transform player;

    public float fireRate = 2f;   // How often meteors drop
    public int level = 1;
    public int maxLevel = 5;

    public float radius = 5f;     // spawn radius around player
    public int meteorsPerWave = 0; // can scale with level
    public int maxMeteorsPerWave = 5;

    private float nextFireTime = 0f;

    public LevelUpButtons levelUpButton;

    void Start()
    {
        levelUpButton.LevelUp(level, maxLevel);
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            SpawnMeteors();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void SpawnMeteors()
    {
        for (int i = 0; i < meteorsPerWave; i++)
        {
            Vector2 targetPos = GetRandomPositionAroundPlayer();

            // Spawn meteor above target
            GameObject meteor = Instantiate(meteorPrefab, targetPos + Vector2.up * 10f, Quaternion.identity);

            // Assign target position for shadow
            Meteor meteorScript = meteor.GetComponent<Meteor>();
            if (meteorScript != null)
                meteorScript.targetPosition = targetPos;
        }
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
