using UnityEngine;
using UnityEngine.UI;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Weapon Stats")]
    public float baseFireRate = 1f;
    public float fireRate = 1f;

    public int level = 0;
    public int maxLevel = 5;

    public float baseDamage = 1f;

    protected float nextFireTime = 0f;
    protected float currentCooldown;

    protected PlayerController player;


    [Header("Weapon UI")]
    public GameObject weaponUIPrefab;
    public Transform weaponUIParent;
    public Sprite weaponIcon;

    protected WeaponUI weaponUI;
    public Slider cooldownSlider;


    protected virtual void Start()
    {
        player = FindObjectOfType<PlayerController>();

        fireRate = baseFireRate;

        nextFireTime = Time.time;
    }


    protected float GetEffectiveFireRate()
    {
        float effectiveFireRate = fireRate;

        if (player != null)
        {
            effectiveFireRate *= player.attackSpeedMultiplier;
        }

        return effectiveFireRate;
    }


    protected float GetCooldown()
    {
        float effectiveFireRate = GetEffectiveFireRate();

        float cooldown = 1f / effectiveFireRate;

        if (player != null)
        {
            cooldown *= player.weaponCooldownMultiplier;
        }

        return cooldown;
    }


    protected float GetWeaponDamage()
    {
        float damage = baseDamage;

        damage *= PlayerStats.GetDamageMultiplier();

        if (player != null)
        {
            damage *= player.weaponDamageMultiplier;

            damage = player.ApplyDamageModifiers(damage);
        }

        return damage;
    }


    protected bool CooldownReady()
    {
        return Time.time >= nextFireTime;
    }


    protected void StartCooldown()
    {
        currentCooldown = GetCooldown();

        nextFireTime = Time.time + currentCooldown;
    }


    protected void UpdateCooldownUI()
    {
        if (cooldownSlider == null)
            return;

        if (Time.time >= nextFireTime)
        {
            cooldownSlider.value = 0f;
            return;
        }

        float remainingTime = nextFireTime - Time.time;

        cooldownSlider.value =
            remainingTime / currentCooldown;
    }


    protected void CreateWeaponUI()
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


    public abstract void LevelUp();
}