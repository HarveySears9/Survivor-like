using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Health")]
    public int maxHP = 10;
    public float hp;

    [Header("Movement")]
    public float speed;
    public float startSpeed;
    public VariableJoystick variableJoystick;

    [Header("References")]
    [SerializeField] private FireBreath fireBreath;
    private AnimateSprite animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    public GameObject deathScreen;
    public GameObject deadPlayer;

    public GameObject[] weapons;
    public GameObject rageAura;

    public HealthBar healthBar;

    [Header("Coins")]
    public int coins = 0;
    public TextMeshProUGUI coinText;
    public float coinMultiplyer = 1f;
    public float coinBonusChance = 0f;

    [Header("Movement State")]
    public bool isMoving = false;
    private bool dead = false;
    public Vector2 moveDirection = Vector2.zero;

    [Header("Contact Damage")]
    float lastDamageTime;
    public float damageInterval = 0.25f;

    [Header("Dragon Altar Buffs")]
    public float attackSpeedMultiplier = 1f;
    public float rageDamageBonus = 0f;
    public float lifestealPercent = 0f;
    public bool isRaging = false;

    [Header("Weapon Buffs")]
    public float weaponCooldownMultiplier = 1f;
    public float weaponDamageMultiplier = 1f;

    [Header("Summon Buffs")]
    public float summonDamageMultiplier = 1f;
    public float summonCooldownMultiplier = 1f;

    [Header("Turret Buffs")]
    public float turretDamageMultiplier = 1f;
    public float turretCooldownMultiplier = 1f;

    [Header("Equipment Buffs")]
    public float equipmentDamageMultiplier = 1f;
    public float equipmentMaxHPMultiplier = 1f;

    [Header("Area Buffs")]
    public float areaSizeMultiplier = 1f;

    public GameObject deathEffect;


    // =========================
    // START
    // =========================

    void Start()
    {
        animator =
            GetComponent<AnimateSprite>();

        spriteRenderer =
            GetComponent<SpriteRenderer>();

        rb =
            GetComponent<Rigidbody2D>();


        // =========================
        // LOAD SAVED DATA
        // =========================

        SaveFile.Data playerData =
            SaveFile.LoadData<SaveFile.Data>();

        if (playerData == null)
        {
            playerData =
                new SaveFile.Data();

            playerData.maxHPLevel = 0;
        }


        // =========================
        // PERMANENT UPGRADES
        // =========================

        maxHP = Mathf.RoundToInt(PlayerStats.GetMaxHP() * equipmentMaxHPMultiplier);

        speed =
            2f *
            PlayerStats.GetSpeedMultiplier();

        startSpeed =
            speed;

        attackSpeedMultiplier =
            PlayerStats.GetAttackSpeedMultiplier();


        // =========================
        // INITIAL HP
        // =========================

        hp =
            maxHP;


        // =========================
        // HEALTH BAR
        // =========================

        healthBar.SetMaxHealth(
            maxHP
        );
    }


    // =========================
    // MOVEMENT
    // =========================

    void FixedUpdate()
    {
        if (!dead)
        {
            moveDirection =
                new Vector2(
                    variableJoystick.Horizontal,
                    variableJoystick.Vertical
                );
        }

        rb.MovePosition(
            rb.position +
            moveDirection *
            speed *
            Time.fixedDeltaTime
        );


        if (moveDirection != Vector2.zero)
        {
            animator.isMoving =
                true;

            isMoving =
                true;


            if (moveDirection.x < 0)
            {
                spriteRenderer.flipX =
                    true;
            }
            else if (moveDirection.x > 0)
            {
                spriteRenderer.flipX =
                    false;
            }
        }
        else
        {
            animator.isMoving =
                false;

            isMoving =
                false;
        }


        // =========================
        // RAGE
        // =========================

        if (rageDamageBonus > 0f)
        {
            isRaging =
                hp <= maxHP * 0.5f;

            if (rageAura != null)
            {
                rageAura.SetActive(
                    isRaging
                );
            }
        }
    }


    // =========================
    // DAMAGE
    // =========================

    public void TakeDamage(float damage)
    {
        hp -= damage;

        healthBar.SetHealth(hp);


        if (hp <= 0)
        {
            if (!dead)
            {
                StartCoroutine(
                    StartDeath()
                );
            }

            dead = true;
        }
    }


    // =========================
    // HEALING
    // =========================

    public void Heal(
        float value,
        bool isFlatAmount
    )
    {
        hp +=
            isFlatAmount
            ? value
            : (value / 100f) * maxHP;

        hp =
            Mathf.Clamp(
                hp,
                0,
                maxHP
            );

        healthBar.SetHealth(hp);
    }


    // =========================
    // MAX HP
    // =========================

    public void IncreaseMaxHP(float percentage, bool healDifference = false)
    {
        int increase =
            Mathf.CeilToInt(maxHP * percentage);

        maxHP += increase;

        if (healDifference)
        {
            hp += increase;
            hp = Mathf.Clamp(hp, 0, maxHP);
        }

        healthBar.SetHealth(hp);
    }


    // =========================
    // DEATH
    // =========================

    public void triggerDeath()
    {
        if (!dead)
        {
            StartCoroutine(
                StartDeath()
            );
        }

        dead = true;
    }


    private IEnumerator StartDeath()
    {
        CapsuleCollider2D col =
            GetComponent<CapsuleCollider2D>();

        if (col != null)
        {
            col.enabled = false;
        }


        // Disable all weapons
        foreach (var weapon in weapons)
        {
            weapon.SetActive(false);
        }


        moveDirection =
            Vector2.zero;

        spriteRenderer.enabled =
            false;


        // Death effect
        if (deathEffect != null)
        {
            Quaternion rot =
                Quaternion.Euler(
                    0,
                    0,
                    Random.Range(
                        0,
                        360
                    )
                );

            Instantiate(
                deathEffect,
                transform.position,
                rot
            );
        }


        deadPlayer.SetActive(
            true
        );


        yield return new WaitForSecondsRealtime(
            2f
        );


        if (deathScreen != null)
        {
            deathScreen.SetActive(
                true
            );
        }

        Time.timeScale =
            0f;
    }


    // =========================
    // COINS
    // =========================

    public void AddCoin(int value)
    {
        coins += value;

        if (Random.value < coinBonusChance)
        {
            coins += 1;
        }

        coinText.text = ":" + coins.ToString();
    }


    public void IncreaseCoinMultiplyer(
        float value
    )
    {
        coinMultiplyer +=
            value;
    }


    // =========================
    // CONTACT DAMAGE
    // =========================

    void OnTriggerEnter2D(
        Collider2D other
    )
    {
        TryTakeContactDamage(
            other
        );
    }


    void OnTriggerStay2D(
        Collider2D other
    )
    {
        if (
            Time.time <
            lastDamageTime +
            damageInterval
        )
        {
            return;
        }

        TryTakeContactDamage(
            other
        );
    }


    void TryTakeContactDamage(
        Collider2D other
    )
    {
        Boss boss =
            other.GetComponent<Boss>();

        if (boss != null)
        {
            TakeDamage(
                boss.damage
            );

            lastDamageTime =
                Time.time;

            return;
        }


        EnemyController enemy =
            other.GetComponent<EnemyController>();

        if (enemy != null)
        {
            TakeDamage(
                enemy.damage
            );

            lastDamageTime =
                Time.time;
        }
    }


    // =========================
    // DAMAGE MODIFIERS
    // =========================

    public float ApplyDamageModifiers(
        float dmg
    )
    {
        float finalDamage =
            dmg;


        // Rage
        if (isRaging)
        {
            finalDamage *=
                (1f + rageDamageBonus);
        }


        return finalDamage;
    }


    // =========================
    // LIFE STEAL
    // =========================

    public void OnDamageDealt(
        float finalDamage
    )
    {
        if (lifestealPercent > 0f)
        {
            float healAmount =
                finalDamage *
                lifestealPercent;

            Heal(
                healAmount,
                true
            );
        }
    }
}