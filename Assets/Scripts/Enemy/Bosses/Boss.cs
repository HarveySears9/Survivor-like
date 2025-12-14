using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public string name;

    public int maxHP = 10;
    protected float health;         // Enemy health
    public float speed = 2f;         // Normal movement speed
    public float damage = 1f;

    public float damageInterval = 1f;
    private float lastDamageTime = 0f;

    public Transform playerTransform; // Reference to the player's position
    private Vector2 direction;        // Direction vector for movement
    private Rigidbody2D rb;           // Rigidbody for physics-based movement

    protected float currentSpeed;       // Current speed (used for slowing effects)
    protected float originalSpeed;      // Original speed for resetting after slow

    protected bool moving = true;
    protected bool isDead = false;

    private bool isFlipped = false;

    public string Name { get { return name; } }

    public HealthBar healthBar;

    private SpriteRenderer spriteRenderer;

    public GameObject[] drops;

    protected virtual void Start()
    {
        // Initialize Rigidbody2D and speed variables
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = speed;
        currentSpeed = speed;

        spriteRenderer = GetComponent<SpriteRenderer>();

        health = maxHP;
    }

    protected virtual void FixedUpdate()
    {
        if (playerTransform != null && moving)
        {
            direction = (playerTransform.position - transform.position).normalized;
            bool flip = direction.x < 0;

            if (flip != isFlipped) // Only flip if the state changes
            {
                isFlipped = flip;

                spriteRenderer.flipX = flip;

                foreach (Transform child in GetComponentsInChildren<Transform>())
                {
                    if (child == transform) continue;

                    Vector3 localPosition = child.localPosition;
                    localPosition.x *= -1; // Flip the X-axis
                    child.localPosition = localPosition;
                }
            }

            rb.MovePosition(rb.position + direction * currentSpeed * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
        {
            Weapon weapon = other.GetComponent<Weapon>();
            if (weapon != null)
            {
                Debug.Log("Enemy hit by weapon!");
                TakeDamage(weapon.damage);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + damageInterval)
            {
                lastDamageTime = Time.time;
                Debug.Log("Enemy hit the player!");
                other.GetComponent<PlayerController>().TakeDamage(damage);
            }
        }
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        healthBar.SetHealth(health);

        if (health <= 0f)
        {
            if (!isDead)
            {
                isDead = true;
                // Trigger the EnemyDeathEventManager
                EnemyDeathEventManager.BossDied(transform.position, drops);
                MissionManager.Instance.AddProgress($"kill_Boss");

                Destroy(gameObject);
            }
        }
    }

    public void ApplySlow(float slowMultiplier, float duration)
    {
        // Reduce the current speed by the multiplier
        currentSpeed = originalSpeed * slowMultiplier;

        // Reset speed after the specified duration
        Invoke(nameof(RemoveSlow), duration);
    }

    private void RemoveSlow()
    {
        // Reset the current speed to the original speed
        currentSpeed = originalSpeed;
    }
}
