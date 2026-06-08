using UnityEngine;

public class DivineAura : MonoBehaviour
{
    public float healAmount = 1f;
    public float cooldownDuration = 5f;

    private float cooldownTimer;

    public GameObject gfx;

    public int level = 0;
    public int maxLevel = 5;

    public PlayerController player;
    public LevelUpButtons levelUpButton;

    [Header("Weapon UI")]
    public GameObject weaponUIPrefab;
    public Transform weaponUIParent;
    public Sprite weaponIcon;

    private WeaponUI weaponUI;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("PlayerController script not found on GameObject!");
        }

        cooldownTimer = cooldownDuration;
        levelUpButton.LevelUp(level, maxLevel);
    }

    public void LevelUp()
    {
        level++;

        if (level == 1)
        {
            gfx.SetActive(true);
            CreateWeaponUI();
        }

        if (level > maxLevel)
            level = maxLevel;

        healAmount++;

        levelUpButton.LevelUp(level, maxLevel);
    }

    void Update()
    {
        // Always tick cooldown (important change)
        cooldownTimer += Time.deltaTime;

        // Heal only if needed + ready
        if (player != null &&
            player.hp < player.maxHP &&
            cooldownTimer >= cooldownDuration)
        {
            HealPlayer();
            cooldownTimer = 0f;
        }

        UpdateCooldownUI();
    }

    void HealPlayer()
    {
        player.Heal(healAmount, true);
    }

    // ================= UI =================

    private void CreateWeaponUI()
    {
        GameObject uiObj = Instantiate(weaponUIPrefab, weaponUIParent);

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

        float progress = 1f - Mathf.Clamp01(cooldownTimer / cooldownDuration);
        weaponUI.cooldownSlider.value = Mathf.Clamp01(progress);
    }
}