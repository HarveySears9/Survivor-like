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
    public Transform playerTransform; // Reference to the player's position
    private Vector2 direction;        // Direction vector for movement
    private Rigidbody2D rb;           // Rigidbody for physics-based movement

    public EnemyType enemyType;

    private float currentSpeed;       // Current speed (used for slowing effects)
    private float originalSpeed;      // Original speed for resetting after slow

    private bool isFlipped = false;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Initialize Rigidbody2D and speed variables
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = speed;
        currentSpeed = speed;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (playerTransform != null)
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


void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy hit the player!");
            // Handle player damage logic here
            other.GetComponent<PlayerController>().TakeDamage(damage);
        }

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

    void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            // Trigger the EnemyDeathEventManager
            EnemyDeathEventManager.EnemyDied(transform.position);
            ChallengeEvents.EnemyDefeated(enemyType);
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
