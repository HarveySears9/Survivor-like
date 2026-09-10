using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinSummoner : MonoBehaviour
{
    [Header("Penguin")]
    public GameObject penguinPrefab;
    private Penguin penguin;

    private bool cooldownStarted = false;

    [Header("Weapon Stats")]
    public float fireRate = 2f;
    public int level = 0;
    public int maxLevel = 5;
    public float range = 10f;
    public float baseDamage = 5f;

    private float nextFireTime = 0f;
    private float currentCooldown;

    public LevelUpButtons levelUpButton;

    public PlayerController player;

    public bool unlocked = true;

    [Header("Weapon UI")]
    public GameObject weaponUIPrefab;
    public Transform weaponUIParent;
    public Sprite weaponIcon;

    private WeaponUI weaponUI;


    void Start()
    {
        SaveFile.Data loadedData = SaveFile.LoadData<SaveFile.Data>();

        // unlocked = loadedData.weaponUnlocks[/* PENGUIN INDEX */];

        if (unlocked)
        {
            // Only enable for B'Rick
            if (loadedData.currentCharacter != 0)
            {
                gameObject.SetActive(false);
                return;
            }

            levelUpButton.LevelUp(level, maxLevel);
        }
    }


    void Update()
    {
        if (level > 0)
        {
            if (Time.time >= nextFireTime && !cooldownStarted)
            {
                cooldownStarted = true;
                penguin.StartAttack();
            }
        }

        UpdateCooldownUI();
    }


    private void SummonPenguin()
    {
        EnsurePenguinExists();

        penguin.StartAttack();
    }

    public void StartCooldown()
    {
        float effectiveFireRate = fireRate * player.attackSpeedMultiplier;

        currentCooldown = 1f / effectiveFireRate;

        nextFireTime = Time.time + currentCooldown;

        cooldownStarted = false;
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

        // Make sure the Penguin exists
        EnsurePenguinExists();

        // Tell the Penguin its current weapon level
        penguin.SetLevel(level);

        levelUpButton.LevelUp(level, maxLevel);
    }


    private void EnsurePenguinExists()
    {
        if (penguin == null)
        {
            GameObject penguinObj =
                Instantiate(
                    penguinPrefab,
                    transform.position,
                    Quaternion.identity
                );

            penguin = penguinObj.GetComponent<Penguin>();
        }
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