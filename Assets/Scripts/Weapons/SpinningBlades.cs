using UnityEngine;

public class SpinningBlades : WeaponBase
{
    [Header("Blades")]
    public GameObject[] level1Blades;
    public GameObject[] level2Blades;
    public GameObject[] level3Blades;
    public GameObject[] level4Blades;
    public GameObject[] level5Blades;

    public GameObject[] allBlades;

    public GameObject level1;
    public GameObject level2;
    public GameObject level3;
    public GameObject level4;
    public GameObject level5;

    [Header("Spin")]
    public float spinSpeed;

    [Header("Level Up")]
    public LevelUpButtons levelUpButton;

    public bool unlocked = true;

    private GameObject[] blades;
    private GameObject currentLevel;

    [Header("Area Size")]
    private Vector3 level1OriginalScale;
    private Vector3 level2OriginalScale;
    private Vector3 level3OriginalScale;
    private Vector3 level4OriginalScale;
    private Vector3 level5OriginalScale;

    private float lastAreaSizeMultiplier = -1f;

    protected override void Start()
    {
        base.Start();

        level = 0;

        // Store the original scale of every blade level
        if (level1 != null)
            level1OriginalScale = level1.transform.localScale;

        if (level2 != null)
            level2OriginalScale = level2.transform.localScale;

        if (level3 != null)
            level3OriginalScale = level3.transform.localScale;

        if (level4 != null)
            level4OriginalScale = level4.transform.localScale;

        if (level5 != null)
            level5OriginalScale = level5.transform.localScale;

        blades = level1Blades;
        currentLevel = level1;

        // Make sure all blade levels start disabled
        DisableAllLevels();

        // Apply the initial area size
        ApplyAreaSize();

        if (levelUpButton != null)
        {
            levelUpButton.LevelUp(
                level,
                maxLevel
            );
        }

        SetUpDamage();
    }

    void FixedUpdate()
    {
        if (level <= 0)
            return;

        // Rotate the main object around the player
        transform.Rotate(
            0f,
            0f,
            spinSpeed *
            Time.deltaTime
        );

        // Rotate each individual blade
        foreach (GameObject blade in blades)
        {
            if (blade == null)
                continue;

            blade.transform.Rotate(
                0f,
                0f,
                4f *
                spinSpeed *
                Time.deltaTime
            );
        }

        SetUpDamage();
    }

    void SetUpDamage()
    {
        float finalDamage =
            GetWeaponDamage();

        foreach (GameObject blade in allBlades)
        {
            if (blade == null)
                continue;

            Weapon weapon =
                blade.GetComponent<Weapon>();

            if (weapon != null)
            {
                weapon.damage =
                    finalDamage;
            }
        }
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

        // Disable the previous level
        DisableAllLevels();

        switch (level)
        {
            case 1:
                blades =
                    level1Blades;

                currentLevel =
                    level1;
                break;

            case 2:
                blades =
                    level2Blades;

                currentLevel =
                    level2;
                break;

            case 3:
                blades =
                    level3Blades;

                currentLevel =
                    level3;
                break;

            case 4:
                blades =
                    level4Blades;

                currentLevel =
                    level4;
                break;

            case 5:
                blades =
                    level5Blades;

                currentLevel =
                    level5;
                break;
        }

        if (currentLevel != null)
        {
            currentLevel.SetActive(true);
        }

        SetUpDamage();

        if (levelUpButton != null)
        {
            levelUpButton.LevelUp(
                level,
                maxLevel
            );
        }
    }

    public void UpdateAreaSize()
    {
        // Force the area size to update
        lastAreaSizeMultiplier = -1f;

        ApplyAreaSize();
    }

    void ApplyAreaSize()
    {
        float multiplier = player != null
            ? player.areaSizeMultiplier
            : 1f;

        // Don't do anything if the multiplier hasn't changed
        if (Mathf.Approximately(
            multiplier,
            lastAreaSizeMultiplier))
        {
            return;
        }

        lastAreaSizeMultiplier =
            multiplier;

        if (level1 != null)
        {
            level1.transform.localScale =
                level1OriginalScale * multiplier;
        }

        if (level2 != null)
        {
            level2.transform.localScale =
                level2OriginalScale * multiplier;
        }

        if (level3 != null)
        {
            level3.transform.localScale =
                level3OriginalScale * multiplier;
        }

        if (level4 != null)
        {
            level4.transform.localScale =
                level4OriginalScale * multiplier;
        }

        if (level5 != null)
        {
            level5.transform.localScale =
                level5OriginalScale * multiplier;
        }
    }

    void DisableAllLevels()
    {
        if (level1 != null)
            level1.SetActive(false);

        if (level2 != null)
            level2.SetActive(false);

        if (level3 != null)
            level3.SetActive(false);

        if (level4 != null)
            level4.SetActive(false);

        if (level5 != null)
            level5.SetActive(false);
    }

    void CreateWeaponUI()
    {
        GameObject uiObj =
            Instantiate(
                weaponUIPrefab,
                weaponUIParent
            );

        WeaponUI weaponUI =
            uiObj.GetComponent<WeaponUI>();

        weaponUI.icon.sprite =
            weaponIcon;

        // Spinning Blades has no cooldown
        weaponUI.cooldownSlider.enabled =
            false;
    }
}