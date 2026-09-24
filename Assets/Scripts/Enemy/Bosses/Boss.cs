using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public string name;

    public int maxHP = 10;
    protected float health;
    public float speed = 2f;
    public float damage = 1f;

    public float damageInterval = 1f;
    private float lastDamageTime = 0f;

    public Transform playerTransform;
    private Vector2 direction;

    protected float currentSpeed;
    protected float originalSpeed;

    protected bool moving = true;
    protected bool isDead = false;

    private bool isFlipped = false;

    public string Name { get { return name; } }

    public HealthBar healthBar;

    private SpriteRenderer spriteRenderer;

    public GameObject[] drops;

    public GameObject deathEffect;

    // --- Slow effect tracking ---
    private Coroutine slowRoutine;

    protected virtual void Start()
    {
        originalSpeed = speed;
        currentSpeed = speed;

        spriteRenderer = GetComponent<SpriteRenderer>();

        health = maxHP;
    }

    protected virtual void FixedUpdate()
    {
        if (playerTransform != null && moving)
        {
            direction =
                (playerTransform.position - transform.position).normalized;

            bool flip = direction.x < 0;

            if (flip != isFlipped)
            {
                isFlipped = flip;

                spriteRenderer.flipX = flip;

                foreach (Transform child in GetComponentsInChildren<Transform>())
                {
                    if (child == transform) continue;

                    Vector3 localPosition = child.localPosition;
                    localPosition.x *= -1;
                    child.localPosition = localPosition;
                }
            }

            // Manual movement instead of Rigidbody2D physics
            transform.position +=
                (Vector3)(direction * currentSpeed * Time.fixedDeltaTime);
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

                other.GetComponent<PlayerController>()
                    .TakeDamage(damage);
            }
        }
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;

        healthBar.SetHealth(health);

        if (health <= 0f && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    protected virtual void Die()
    {
        // Spawn death effect
        if (deathEffect != null)
        {
            Quaternion rot =
                Quaternion.Euler(
                    0,
                    0,
                    Random.Range(0, 360)
                );

            Instantiate(
                deathEffect,
                transform.position,
                rot
            );
        }

        EnemyDeathEventManager.BossDied(
            transform.position,
            drops
        );

        MissionManager.Instance.AddProgress(
            $"kill_Boss"
        );

        Destroy(gameObject);
    }

    // --- SLOW EFFECT HANDLING ---

    public void ApplySlow(float slowAmount, float duration)
    {
        if (slowRoutine != null)
            StopCoroutine(slowRoutine);

        slowRoutine =
            StartCoroutine(
                SlowEffect(slowAmount, duration)
            );
    }

    private IEnumerator SlowEffect(
        float slowAmount,
        float duration
    )
    {
        slowAmount = Mathf.Clamp01(slowAmount);

        currentSpeed =
            originalSpeed * slowAmount;

        if (spriteRenderer != null)
        {
            yield return StartCoroutine(
                FadeColor(
                    spriteRenderer.color,
                    Color.cyan,
                    0.1f
                )
            );
        }

        yield return new WaitForSeconds(duration);

        currentSpeed = originalSpeed;

        if (spriteRenderer != null)
        {
            yield return StartCoroutine(
                FadeColor(
                    spriteRenderer.color,
                    Color.white,
                    0.1f
                )
            );
        }

        slowRoutine = null;
    }

    private IEnumerator FadeColor(
        Color from,
        Color to,
        float time
    )
    {
        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;

            spriteRenderer.color =
                Color.Lerp(
                    from,
                    to,
                    elapsed / time
                );

            yield return null;
        }

        spriteRenderer.color = to;
    }
}