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

    private float currentCooldown;

    [Header("Weapon UI")]
    public GameObject weaponUIPrefab;
    public Transform weaponUIParent;
    public Sprite weaponIcon;

    private WeaponUI weaponUI;

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
            currentCooldown = 1f / effectiveFireRate;
            nextFireTime = Time.time + currentCooldown;
        }

        UpdateCooldownUI();
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

        if (level == 1)
        {
            CreateWeaponUI();
        }

        if (level > maxLevel) level = maxLevel;

        levelUpButton.LevelUp(level, maxLevel);

        // increase meteors per wave with level
        meteorsPerWave++;
        if (meteorsPerWave > maxMeteorsPerWave)
            meteorsPerWave = maxMeteorsPerWave;
    }

    private void CreateWeaponUI()
    {
        GameObject uiObj =
            Instantiate(weaponUIPrefab, weaponUIParent);

        weaponUI = uiObj.GetComponent<WeaponUI>();

        weaponUI.icon.sprite = weaponIcon;

        weaponUI.cooldownSlider.minValue = 0f;
        weaponUI.cooldownSlider.maxValue = 1f;
        weaponUI.cooldownSlider.value = 0f;
    }

    private void UpdateCooldownUI()
    {
        if (weaponUI == null)
            return;

        if (Time.time >= nextFireTime)
        {
            weaponUI.cooldownSlider.value = 0f;
            return;
        }

        float remaining = nextFireTime - Time.time;

        weaponUI.cooldownSlider.value =
            remaining / currentCooldown;
    }
}
