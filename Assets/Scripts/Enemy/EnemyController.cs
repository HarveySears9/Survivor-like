using UnityEngine;
using System.Collections;

public enum EnemyType
{
    Goblin,
    Skeleton,
    Boss,
    Other
}

public class EnemyController : MonoBehaviour
{
    public float health = 1f;
    public float speed = 2f;
    public float damage = 1f;

    public Transform playerTransform;

    private Vector2 direction;
    private Rigidbody2D rb;

    public EnemyType enemyType;

    private float currentSpeed;
    private float originalSpeed;

    private bool isFlipped = false;
    private bool isDead = false;

    private SpriteRenderer spriteRenderer;
    private Collider2D myCollider;

    // --- Separation ---
    [SerializeField] private float separationRadius = 0.15f;
    [SerializeField] private float separationStrength = 0.1f;

    private Vector2 separationForce;
    private float separationTimer;

    // Increased from 0.1 to 0.2
    [SerializeField] private float separationUpdateRate = 0.2f;

    // Reusable array so we don't allocate a new array every search
    private Collider2D[] separationResults = new Collider2D[16];

    public GameObject deathEffect;

    // --- Slow effect tracking ---
    private Coroutine slowRoutine;

    private Vector2 smoothDirection;

    private Vector2 targetDirection;
    private int directionUpdateCounter = 0;

    [SerializeField] private int directionUpdateFrames = 5;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalSpeed = speed;
        currentSpeed = speed;

        // Stagger separation updates between enemies
        separationTimer = Random.Range(0f, separationUpdateRate);

        // Get initial direction immediately
        if (playerTransform != null)
        {
            targetDirection =
                (playerTransform.position - transform.position).normalized;
        }
    }

    void FixedUpdate()
    {
        if (playerTransform == null) return;

        // Only recalculate direction every few physics frames
        directionUpdateCounter++;

        if (directionUpdateCounter >= directionUpdateFrames)
        {
            directionUpdateCounter = 0;

            targetDirection =
                (playerTransform.position - transform.position).normalized;
        }

        separationTimer -= Time.fixedDeltaTime;

        if (separationTimer <= 0f)
        {
            UpdateSeparation();
            separationTimer = separationUpdateRate;
        }

        Vector2 finalDir =
            (targetDirection + separationForce * separationStrength).normalized;

        // Continue smoothing every physics frame
        smoothDirection =
            Vector2.Lerp(smoothDirection, finalDir, 0.1f);

        rb.MovePosition(
            rb.position +
            smoothDirection * currentSpeed * Time.fixedDeltaTime
        );

        HandleFlip(targetDirection.x);
    }

    private void UpdateSeparation()
    {
        separationForce = Vector2.zero;

        int count = Physics2D.OverlapCircleNonAlloc(
            transform.position,
            separationRadius,
            separationResults
        );

        for (int i = 0; i < count; i++)
        {
            Collider2D c = separationResults[i];

            if (c != null &&
                c != myCollider &&
                c.CompareTag("Enemy"))
            {
                Vector2 away =
                    (Vector2)(transform.position - c.transform.position);

                float dist = away.magnitude;

                if (dist > 0f)
                {
                    separationForce += away.normalized / dist;
                }
            }
        }
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
                TakeDamage(weapon.damage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;

        if (health <= 0f)
        {
            isDead = true;

            GameObject effect = BloodEffectPool.Instance.Get();

            effect.transform.position = transform.position;
            effect.transform.rotation =
                Quaternion.Euler(0, 0, Random.Range(0, 360));

            EnemyDeathEventManager.EnemyDied(transform.position);

            MissionManager.Instance.AddProgress(
                $"kill_{enemyType}"
            );

            Destroy(gameObject);
        }
    }

    // --- SLOW EFFECT HANDLING ---

    public void ApplySlow(float slowAmount, float duration)
    {
        if (slowRoutine != null)
            StopCoroutine(slowRoutine);

        slowRoutine = StartCoroutine(
            SlowEffect(slowAmount, duration)
        );
    }

    private IEnumerator SlowEffect(float slowAmount, float duration)
    {
        slowAmount = Mathf.Clamp01(slowAmount);

        currentSpeed = originalSpeed * slowAmount;

        if (spriteRenderer != null)
            yield return StartCoroutine(
                FadeColor(
                    spriteRenderer.color,
                    Color.cyan,
                    0.1f
                )
            );

        yield return new WaitForSeconds(duration);

        currentSpeed = originalSpeed;

        if (spriteRenderer != null)
            yield return StartCoroutine(
                FadeColor(
                    spriteRenderer.color,
                    Color.white,
                    0.1f
                )
            );

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