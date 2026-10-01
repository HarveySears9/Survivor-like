using UnityEngine;

public class DivineAura : EquipmentBase
{
    [Header("Healing")]
    public float healAmount = 1f;
    public float cooldownDuration = 5f;

    private float cooldownTimer;

    [Header("Visual")]
    public GameObject gfx;

    [Header("Player")]
    public PlayerController player;

    [Header("Equipment UI")]
    public GameObject weaponUIPrefab;
    public Transform weaponUIParent;
    public Sprite weaponIcon;

    private WeaponUI weaponUI;

    protected override void Start()
    {
        if (player == null)
        {
            Debug.LogError(
                "PlayerController script not found on GameObject!"
            );
        }

        if (!PlayerDataManager.Instance.data.equipmentUnlocks[1])
        {
            return;
        }

        cooldownTimer =
            cooldownDuration;

        base.Start();
    }

    protected override void ApplyLevel()
    {
        healAmount++;

        if (level == 1)
        {
            if (gfx != null)
            {
                gfx.SetActive(true);
            }

            CreateWeaponUI();
        }
    }

    private void Update()
    {
        cooldownTimer +=
            Time.deltaTime;

        if (player != null &&
            player.hp < player.maxHP &&
            cooldownTimer >= cooldownDuration)
        {
            HealPlayer();

            cooldownTimer = 0f;
        }

        UpdateCooldownUI();
    }

    private void HealPlayer()
    {
        player.Heal(
            healAmount,
            true
        );
    }

    // =========================
    // UI
    // =========================

    private void CreateWeaponUI()
    {
        GameObject uiObj =
            Instantiate(
                weaponUIPrefab,
                weaponUIParent
            );

        weaponUI =
            uiObj.GetComponent<WeaponUI>();

        weaponUI.icon.sprite =
            weaponIcon;

        weaponUI.cooldownSlider.minValue =
            0f;

        weaponUI.cooldownSlider.maxValue =
            1f;

        weaponUI.cooldownSlider.value =
            0f;
    }

    private void UpdateCooldownUI()
    {
        if (weaponUI == null)
            return;

        float progress =
            1f -
            Mathf.Clamp01(
                cooldownTimer /
                cooldownDuration
            );

        weaponUI.cooldownSlider.value =
            Mathf.Clamp01(progress);
    }
}