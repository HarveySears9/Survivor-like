using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DragonTail : MonoBehaviour
{
    [Header("Tail")]
    public GameObject tailPrefab;

    [Header("Weapon Stats")]
    public float baseFireRate = 1.5f;
    public float fireRate = 1.5f;
    public int level = 0;
    public int maxLevel = 5;

    public float baseDamage = 1f;

    [Header("Attack")]
    public float attackDuration = 0.45f;

    private bool isAttacking = false;

    private float nextFireTime = 0f;
    private float currentCooldown;

    private PlayerController player;

    private Vector2 lastAttackDirection = Vector2.right;

    [Header("Weapon UI")]
    public GameObject weaponUIPrefab;
    public Transform weaponUIParent;
    public Sprite weaponIcon;

    private WeaponUI weaponUI;
    public Slider cooldownSlider;

    public bool unlocked = true;

    public LevelUpButtons levelUpButton;


    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        level = 0;

        fireRate = baseFireRate;

        unlocked = PlayerDataManager.Instance.data.weaponUnlocks[9];

        if (unlocked)
        {
            levelUpButton.LevelUp(level, maxLevel);
        }

        nextFireTime = Time.time;
    }


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

        if (Time.time >= nextFireTime && !isAttacking)
        {
            StartCoroutine(Attack());

            currentCooldown = 1f / effectiveFireRate;
            nextFireTime = Time.time + currentCooldown;
        }

        UpdateCooldownUI();

        // Remember the direction B'Rick is moving
        if (player.moveDirection != Vector2.zero)
        {
            lastAttackDirection = player.moveDirection;
        }
    }


    private IEnumerator Attack()
    {
        isAttacking = true;

        if (player == null)
        {
            isAttacking = false;
            yield break;
        }

        // Opposite direction to movement
        Vector2 attackDirection = -lastAttackDirection;

        // Create the tail as a child of B'Rick
        GameObject tail = Instantiate(
            tailPrefab,
            transform
        );

        // Position around B'Rick's waist / tail bone
        tail.transform.localPosition = new Vector3(0f, -1f, 0f);

        // Rotate tail to face the opposite direction
        float angle =
            Mathf.Atan2(attackDirection.y, attackDirection.x)
            * Mathf.Rad2Deg;

        // Tail sprite points UP by default
        tail.transform.localRotation =
            Quaternion.Euler(0, 0, angle - 90f);

        // Apply damage
        float finalDamage =
            baseDamage * PlayerStats.GetDamageMultiplier();

        Weapon weapon =
            tail.GetComponent<Weapon>();

        if (weapon != null)
        {
            weapon.damage =
                player.ApplyDamageModifiers(finalDamage);
        }

        // Keep tail alive for the duration of the animation
        yield return new WaitForSeconds(attackDuration);

        Destroy(tail);

        isAttacking = false;
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
                fireRate = baseFireRate;
                break;

            case 2:
                fireRate = baseFireRate * 1.15f;
                break;

            case 3:
                fireRate = baseFireRate * 1.3f;
                break;

            case 4:
                fireRate = baseFireRate * 1.45f;
                break;

            case 5:
                fireRate = baseFireRate * 1.6f;
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