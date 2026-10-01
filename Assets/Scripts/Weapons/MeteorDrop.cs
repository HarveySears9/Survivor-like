using UnityEngine;
using System.Collections;

public class MeteorDrop : WeaponBase
{
    [Header("Meteor Drop")]
    public GameObject meteorPrefab;

    public float radius = 5f;

    public int meteorsPerWave = 0;
    public int maxMeteorsPerWave = 5;

    private bool firing = false;

    public LevelUpButtons levelUpButton;

    public bool unlocked = true;

    protected override void Start()
    {
        base.Start();

        unlocked =
            PlayerDataManager.Instance.data.weaponUnlocks[4];

        if (unlocked)
        {
            levelUpButton.LevelUp(level, maxLevel);
        }
    }

    void Update()
    {
        if (level <= 0)
            return;

        if (!firing && CooldownReady())
        {
            StartCoroutine(SpawnMeteors());
        }

        UpdateCooldownUI();
    }

    IEnumerator SpawnMeteors()
    {
        firing = true;

        // Start the weapon cooldown when the wave begins
        StartCooldown();

        for (int i = 0; i < meteorsPerWave; i++)
        {
            Vector2 targetPos =
                GetRandomPositionAroundPlayer();

            GameObject meteor =
                Instantiate(
                    meteorPrefab,
                    targetPos + Vector2.up * 10f,
                    Quaternion.identity
                );

            Meteor meteorScript =
                meteor.GetComponent<Meteor>();

            if (meteorScript != null)
            {
                meteorScript.targetPosition =
                    targetPos;

                meteorScript.damage =
                    GetWeaponDamage();

                meteorScript.areaSizeMultiplier =
                    player.areaSizeMultiplier;
            }

            float randomDelay =
                Random.Range(0.1f, 0.5f);

            yield return new WaitForSeconds(randomDelay);
        }

        firing = false;
    }

    Vector2 GetRandomPositionAroundPlayer()
    {
        Vector2 offset =
            Random.insideUnitCircle * radius;

        return (Vector2)player.transform.position + offset;
    }

    public override void LevelUp()
    {
        level++;

        if (level > maxLevel)
            level = maxLevel;

        if (level == 1)
        {
            CreateWeaponUI();
        }

        levelUpButton.LevelUp(level, maxLevel);

        meteorsPerWave++;

        if (meteorsPerWave > maxMeteorsPerWave)
        {
            meteorsPerWave =
                maxMeteorsPerWave;
        }
    }
}