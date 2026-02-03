using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems; // For pointer events
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public int maxHP = 10;
    public float hp;
    public float speed;
    public float startSpeed;
    public VariableJoystick variableJoystick;
    [SerializeField] private FireBreath fireBreath;
    private AnimateSprite animator;
    private SpriteRenderer spriteRenderer; // To control flipping of the sprite

    private Rigidbody2D rb;

    public GameObject deathScreen;
    public GameObject deadPlayer;

    public GameObject[] weapons;

    public HealthBar healthBar;

    public int coins = 0;
    public TextMeshProUGUI coinText;

    public float coinMultiplyer = 1f;

    public bool isMoving = false;
    private bool dead = false;
    public Vector2 moveDirection = Vector2.zero;

    float lastDamageTime;
    public float damageInterval = 0.25f;

    [Header("Dragon Altar Buffs")]
    public float attackSpeedMultiplier = 1f;
    public float rageDamageBonus = 0f;   // % bonus when low HP (0.3 = +30%)
    public float lifestealPercent = 0f;  // % of damage dealt (0.05 = 5%)
    public bool isRaging = false;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<AnimateSprite>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // Load saved data
        SaveFile.Data playerData = SaveFile.LoadData<SaveFile.Data>();

        if (playerData == null)
        {
            playerData = new SaveFile.Data();
            playerData.maxHPLevel = 0;
        }

        // Calculate Max HP based on upgrade level
        maxHP = maxHP + playerData.maxHP; // +2 HP per upgrade level
        speed = speed * (1f + playerData.speedLevel * 0.05f);

        startSpeed = speed;

        hp = maxHP; // Set current HP to max HP

        // Find and initialize the health bar
        healthBar.SetMaxHealth(maxHP);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // For 2D movement (X and Y only)
        if (!dead)
        {
            moveDirection = new Vector2(variableJoystick.Horizontal, variableJoystick.Vertical);
        }

        // Move the player in the direction specified by moveDirection
        rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);

        if (moveDirection != Vector2.zero)
        {
            if(fireBreath != null)
            {
                fireBreath.moveDirection = moveDirection;
            }
            animator.isMoving = true;
            isMoving = true;

            // Flip sprite based on movement direction
            if (moveDirection.x < 0)
            {
                spriteRenderer.flipX = true; // Flip sprite when moving left
            }
            else if (moveDirection.x > 0)
            {
                spriteRenderer.flipX = false; // Keep sprite normal when moving right
            }
        }
        else
        {
            animator.isMoving = false;
            isMoving = false;
        }

        //player is raging when Hp is below half
        isRaging = hp <= maxHP * 0.5f;


    }

    public void TakeDamage(float damage)
    {
        hp -= damage;

        // Update health bar
        healthBar.SetHealth(hp);

        // Check if the player is dead
        if (hp <= 0)
        {
            if(!dead)
            {
                StartCoroutine(StartDeath());
            }
            dead = true;
        }
    }

    public void Heal(float value, bool isFlatAmount)
    {
        hp += isFlatAmount ? value : (value / 100f) * maxHP;
        hp = Mathf.Clamp(hp, 0, maxHP); // Clamp to prevent exceeding maxHP
        healthBar.SetHealth(hp);
    }

    public void IncreaseMaxHP(float percentage)
    {
        int increase = Mathf.CeilToInt(maxHP * percentage);
        maxHP += increase;
        healthBar.SetHealth(hp);
    }

    private IEnumerator StartDeath()
    {
        // Disable Collider (for building interation)
        CapsuleCollider2D col = GetComponent<CapsuleCollider2D>();
        if (col != null)
        {
            col.enabled = false;
        }


        // Disable all weapons
        foreach (var weapon in weapons)
        {
            weapon.SetActive(false);
        }

        SaveFile.Data loadedData = SaveFile.LoadData<SaveFile.Data>();
        loadedData.coins += coins;
        SaveFile.SaveData(loadedData);

        moveDirection = Vector2.zero;

        spriteRenderer.enabled = false;
        deadPlayer.SetActive(true);

        deadPlayer.GetComponent<SpriteRenderer>().flipX = spriteRenderer.flipX;

        // Wait for 2 seconds in real-world time
        yield return new WaitForSecondsRealtime(2f);

        // Activate the death screen
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
        }

        Time.timeScale = 0f;
    }


    public void AddCoin(int value)
    {
        coins += Mathf.FloorToInt(value * coinMultiplyer);
        coinText.text = ":" + coins.ToString();
    }

    public void IncreaseCoinMultiplyer(float value)
    {
        coinMultiplyer += value;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        TryTakeContactDamage(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (Time.time < lastDamageTime + damageInterval) return;
        TryTakeContactDamage(other);
    }

    void TryTakeContactDamage(Collider2D other)
    {
        // Boss damage
        Boss boss = other.GetComponent<Boss>();
        if (boss != null)
        {
            TakeDamage(boss.damage);
            lastDamageTime = Time.time;
            return;
        }

        // Normal enemy damage
        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            TakeDamage(enemy.damage);
            lastDamageTime = Time.time;
        }
    }

}