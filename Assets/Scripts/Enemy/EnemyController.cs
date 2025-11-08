using UnityEngine;

public enum EnemyType
{
    Goblin,
    Skeleton,
    Boss,
    Other
    // add more as you go
}


public class EnemyController : MonoBehaviour
{
    public float health = 1f;         // Enemy health
    public float speed = 2f;         // Normal movement speed
    public float damage = 1f;
    public float damageInterval = 1f;
    private float lastDamageTime = 0f;

    public Transform playerTransform; // Reference to the player's position
    private Vector2 direction;        // Direction vector for movement
    private Rigidbody2D rb;           // Rigidbody for physics-based movement

    public EnemyType enemyType;

    private float currentSpeed;       // Current speed (used for slowing effects)
    private float originalSpeed;      // Original speed for resetting after slow

    private bool isFlipped = false;

    private bool isDead = false;

    private SpriteRenderer spriteRenderer;


    private float separationRadius = 0.15f;
    private float separationStrength = 0.1f;

    void Start()
    {
        // Initialize Rigidbody2D and speed variables
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = speed;
        currentSpeed = speed;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private Vector2 smoothDirection;

    void FixedUpdate()
    {
        if (playerTransform == null) return;

        Vector2 targetDir = (playerTransform.position - transform.position).normalized;

        // --- Separation behaviour ---
        Vector2 separationForce = Vector2.zero;
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, separationRadius);

        foreach (var c in nearbyEnemies)
        {
            if (c != null && c != GetComponent<Collider2D>() && c.CompareTag("Enemy"))
            {
                Vector2 away = (Vector2)(transform.position - c.transform.position);
                float dist = away.magnitude;
                if (dist > 0f)
                    separationForce += away.normalized / dist;
            }
        }

        Vector2 finalDir = (targetDir + separationForce * separationStrength).normalized;

        // Smooth transition instead of snapping
        smoothDirection = Vector2.Lerp(smoothDirection, finalDir, 0.1f);

        rb.MovePosition(rb.position + smoothDirection * currentSpeed * Time.fixedDeltaTime);

        HandleFlip(targetDir.x);
    }


    void HandleFlip(float dirX)
    {
        bool flip = dirX < 0;
        if (flip != isFlipped)
        {
            isFlipped = flip;
            spriteRenderer.flipX = flip;

            foreach (Transform child in GetComponentsInChildren<Transform>())
            {
                if (child == transform) continue;
                Vector3 localPos = child.localPosition;
                localPos.x *= -1;
                child.localPosition = localPos;
            }
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
                Debug.Log("Enemy dealing periodic damage to player!");
                other.GetComponent<PlayerController>().TakeDamage(damage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; // already dead, ignore further damage

        health -= damage;
        if (health <= 0f)
        {
            isDead = true;

            // Trigger the EnemyDeathEventManager
            EnemyDeathEventManager.EnemyDied(transform.position);

            MissionManager.Instance.AddProgress($"kill_{enemyType}");

            Destroy(gameObject);
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
