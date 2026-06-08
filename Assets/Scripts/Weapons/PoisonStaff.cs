using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonStaff : MonoBehaviour
{
    public float fireRate = 2f;
    public int level = 0;
    public int maxLevel = 5;
    //public float range = 10f;

    private float nextFireTime = 0f;

    public AnimateSprite playerAnimator;
    //public AnimateImage levelUpButtonAnimator;

    public Sprite[] staff1, staff1moving, staff2, staff2moving, staff3, staff3moving, staff4, staff4moving, staff5, staff5moving;

    public LevelUpButtons levelUpButton;

    public SpriteRenderer sr;
    public PlayerController player;

    public float baseDamage = 1f;

    public bool unlocked = false;

    public GameObject staffOrbitPrefab;

    private bool staffActive = false;

    private float currentCooldown;

    [Header("Weapon UI")]
    public GameObject weaponUIPrefab;
    public Transform weaponUIParent;
    public Sprite weaponIcon;

    private WeaponUI weaponUI;

    void Start()
    {
        SaveFile.Data loadedData = SaveFile.LoadData<SaveFile.Data>();

        //unlocked = loadedData.weaponUnlocks[1];
        unlocked = true;

        if (unlocked)
        {
            levelUpButton.LevelUp(level, maxLevel);
            UpdateSprites();
        }
    }

    void Update()
    {
        if (level > 0)
        {
            if (level > 0 && !staffActive && Time.time >= nextFireTime)
            {
                SpawnStaff();
            }
        }

        playerAnimator.isMoving = player.isMoving;

        // Flip sprite
        if (player.moveDirection.x < 0)
            sr.flipX = true;
        else if (player.moveDirection.x > 0)
            sr.flipX = false;

        UpdateCooldownUI();
    }

    void SpawnStaff()
    {
        GameObject staff = Instantiate(staffOrbitPrefab, transform.position, Quaternion.identity);

        PlayerStaffOrbit orbit = staff.GetComponent<PlayerStaffOrbit>();

        if (orbit != null)
        {
            orbit.Initialize(transform, this, level);
        }

        staffActive = true;
        sr.enabled = false; // hide held staff
    }

    public void StaffFinished()
    {
        staffActive = false;
        sr.enabled = true;

        float effectiveFireRate = fireRate * player.attackSpeedMultiplier;
        currentCooldown = 1f / effectiveFireRate;
        nextFireTime = Time.time + currentCooldown;
    }

    public void LevelUp()
    {
        level++;

        if (level == 1)
        {
            CreateWeaponUI();
        }

        if (level > maxLevel)
            level = maxLevel;

        levelUpButton.LevelUp(level, maxLevel);

        UpdateSprites();
    }

    void UpdateSprites()
    {
        switch (level)
        {
            case 1:
                playerAnimator.spriteArray = staff1;
                playerAnimator.moveArray = staff1moving;
                //levelUpButtonAnimator.spriteArray = hammer2;
                break;

            case 2:
                playerAnimator.spriteArray = staff2;
                playerAnimator.moveArray = staff2moving;
                //levelUpButtonAnimator.spriteArray = hammer3;
                break;

            case 3:
                playerAnimator.spriteArray = staff3;
                playerAnimator.moveArray = staff3moving;
                //levelUpButtonAnimator.spriteArray = hammer4;
                break;

            case 4:
                playerAnimator.spriteArray = staff4;
                playerAnimator.moveArray = staff4moving;
                //levelUpButtonAnimator.spriteArray = hammer5;
                break;

            case 5:
                playerAnimator.spriteArray = staff5;
                playerAnimator.moveArray = staff5moving;
                break;
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
