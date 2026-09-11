using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bow : MonoBehaviour
{
    [Header("Projectile")]
    public GameObject arrowPrefab;

    [Header("Weapon Stats")]
    public float baseFireRate = 3f;
    public float fireRate = 3f;
    public int level = 0;
    public int maxLevel = 5;

    public float baseDamage = 1f;

    public int arrowsPerBurst = 1;
    public float burstDelay = 0.12f;

    private bool isBursting = false;

    private float nextFireTime = 0f;
    private float currentCooldown;

    private PlayerController player;

    private Vector2 lastFireDirection = Vector2.right;

    [Header("Weapon UI")]
    public GameObject weaponUIPrefab;
    public Transform weaponUIParent;
    public Sprite weaponIcon;

    private WeaponUI weaponUI;
    public Slider cooldownSlider;


    public bool unlocked = true;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        level = 0;

        fireRate = baseFireRate;

        unlocked = PlayerDataManager.Instance.data.weaponUnlocks[8];

        if (unlocked)
        {
            levelUpButton.LevelUp(level, maxLevel);
        }

        nextFireTime = Time.time;
    }


    public LevelUpButtons levelUpButton;


    void Update()
    {
        // Don't attack until the weapon has actually been obtained
        if (level <= 0)
            return;

        float effectiveFireRate = fireRate;

        if (player != null)
        {
            effectiveFireRate *= player.attackSpeedMultiplier;
        }

        if (Time.time >= nextFireTime && !isBursting)
        {
            StartCoroutine(FireBurst());

            currentCooldown = 1f / effectiveFireRate;
            nextFireTime = Time.time + currentCooldown;
        }

        UpdateCooldownUI();

        if (player.moveDirection != Vector2.zero)
        {
            lastFireDirection = player.moveDirection;
        }
    }

    private IEnumerator FireBurst()
    {
        isBursting = true;

        for (int i = 0; i < arrowsPerBurst; i++)
        {
            Fire();

            if (i < arrowsPerBurst - 1)
            {
                yield return new WaitForSeconds(burstDelay);
            }
        }

        isBursting = false;
    }

    private void Fire()
    {
        if (player == null)
            return;

        Vector2 fireDirection = lastFireDirection;


        GameObject arrow = Instantiate(
            arrowPrefab,
            transform.position,
            Quaternion.identity
        );

        float angleToRotate = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;

        arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleToRotate));

        arrow.GetComponent<Fireball>().Initialize(Vector2.right);

        float finalDamage =
            baseDamage * PlayerStats.GetDamageMultiplier();

        arrow.GetComponent<Weapon>().damage =
            player.ApplyDamageModifiers(finalDamage);
    }


    public void LevelUp()
    {
        level++;

        if (level > maxLevel)
            level = maxLevel;

        if (level == 1)
        {
            CreateWeaponUI();
        }

        levelUpButton.LevelUp(level, maxLevel);

        switch (level)
        {
            case 1:
                arrowsPerBurst = 1;
                break;

            case 2:
                arrowsPerBurst = 2;
                break;

            case 3:
                arrowsPerBurst = 3;
                break;

            case 4:
                arrowsPerBurst = 4;
                break;

            case 5:
                arrowsPerBurst = 5;
                break;
        }
    }


    private void UpdateCooldownUI()
    {
        if (cooldownSlider == null)
            return;

        if (Time.time >= nextFireTime)
        {
            cooldownSlider.value = 0f;
            return;
        }

        float remainingTime =
            nextFireTime - Time.time;

        cooldownSlider.value =
            remainingTime / currentCooldown;
    }


    private void CreateWeaponUI()
    {
        GameObject uiObj =
            Instantiate(
                weaponUIPrefab,
                weaponUIParent
            );

        weaponUI =
            uiObj.GetComponent<WeaponUI>();

        weaponUI.icon.sprite = weaponIcon;

        cooldownSlider =
            weaponUI.cooldownSlider;
    }
}