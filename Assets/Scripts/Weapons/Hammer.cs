using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public GameObject hammerPrefab;   // Hammer projectile
    public float fireRate = 2f;
    public int level = 0;
    public int maxLevel = 5;
    public float range = 10f;

    private float nextFireTime = 0f;

    public AnimateSprite playerAnimator;
    //public AnimateImage levelUpButtonAnimator;

    public Sprite[] hammer1, hammer1moving, hammer2, hammer2moving, hammer3, hammer3moving, hammer4, hammer4moving, hammer5, hammer5moving;

    public LevelUpButtons levelUpButton;

    public SpriteRenderer sr;
    public PlayerController player;

    public float baseDamage = 5f;

    private bool hammerActive = false;

    public bool unlocked = false;

    private float currentCooldown;

    [Header("Weapon UI")]
    public GameObject weaponUIPrefab;
    public Transform weaponUIParent;
    public Sprite weaponIcon;

    private WeaponUI weaponUI;

    void Start()
    {
        SaveFile.Data loadedData = SaveFile.LoadData<SaveFile.Data>();

        unlocked = loadedData.weaponUnlocks[0];

        if (unlocked)
        {
            //SaveFile.Data loadedData = SaveFile.LoadData<SaveFile.Data>();

            // Only enable for B'Rick (same as sword logic)
            if (loadedData.currentCharacter != 0)
            {
                gameObject.SetActive(false);
                return;
            }


            levelUpButton.LevelUp(level, maxLevel);
            UpdateSprites();
        }
    }

    void Update()
    {
        if (level > 0)
        {
            if (Time.time >= nextFireTime)
            {
                if (!hammerActive)
                {
                    ThrowAtTargets();
                }
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

    void ThrowAtTargets()
    {
        Collider2D[] targetsInRange = Physics2D.OverlapCircleAll(transform.position, range);

        List<Transform> enemyTargets = new List<Transform>();

        foreach (var col in targetsInRange)
        {
            if (col.CompareTag("Enemy"))
            {
                enemyTargets.Add(col.transform);
            }
        }

        if (enemyTargets.Count == 0) return;

        Transform target = enemyTargets[0]; // just pick first (or closest later)

        GameObject hammerObj = Instantiate(hammerPrefab, transform.position, Quaternion.identity);

        PlayerHammer hammer = hammerObj.GetComponent<PlayerHammer>();

        if (hammer != null)
        {
            hammer.Initialize(target.position, transform, this, level);
            float finalDamage = baseDamage * PlayerStats.GetDamageMultiplier();

            finalDamage = player.ApplyDamageModifiers(finalDamage);

            hammer.damage = finalDamage;

            Weapon hammerWeapon = hammerObj.GetComponent<Weapon>();
            if(hammerWeapon != null)
            {
                hammerWeapon.damage = finalDamage;
            }
        }

        hammerActive = true;
        sr.enabled = false; // hide weapon sprite while thrown
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
                playerAnimator.spriteArray = hammer1;
                playerAnimator.moveArray = hammer1moving;
                //levelUpButtonAnimator.spriteArray = hammer2;
                break;

            case 2:
                playerAnimator.spriteArray = hammer2;
                playerAnimator.moveArray = hammer2moving;
                //levelUpButtonAnimator.spriteArray = hammer3;
                break;

            case 3:
                playerAnimator.spriteArray = hammer3;
                playerAnimator.moveArray = hammer3moving;
                //levelUpButtonAnimator.spriteArray = hammer4;
                break;

            case 4:
                playerAnimator.spriteArray = hammer4;
                playerAnimator.moveArray = hammer4moving;
                //levelUpButtonAnimator.spriteArray = hammer5;
                break;

            case 5:
                playerAnimator.spriteArray = hammer5;
                playerAnimator.moveArray = hammer5moving;
                break;
        }
    }

    public void HammerReturned()
    {
        hammerActive = false;
        sr.enabled = true;

        float effectiveFireRate = fireRate * player.attackSpeedMultiplier;
        currentCooldown = 1f / effectiveFireRate;
        nextFireTime = Time.time + currentCooldown;
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
