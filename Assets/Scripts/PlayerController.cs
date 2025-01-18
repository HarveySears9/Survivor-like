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

    public GameObject deathScreen;
    public GameObject deadPlayer;

    public GameObject[] weapons;

    public HealthBar healthBar;

    public int coins = 0;
    public TextMeshProUGUI coinText;

    public bool isMoving = false;
    private bool dead = false;
    public Vector2 moveDirection = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<AnimateSprite>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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
        transform.Translate(moveDirection * speed * Time.deltaTime);

        if (moveDirection != Vector2.zero)
        {
            fireBreath.moveDirection = moveDirection;
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


    private IEnumerator StartDeath()
    {
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
        coins += value;
        coinText.text = "Coins:" + coins.ToString();
    }

}