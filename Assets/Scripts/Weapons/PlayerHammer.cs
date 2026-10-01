using System.Collections;
using UnityEngine;

public class PlayerHammer : MonoBehaviour
{
    [Header("Hammer")]
    public float speed = 6f;
    public float damage = 5f;

    [Header("Return")]
    public float returnDelay = 0.5f;

    [Header("Visuals")]
    public GameObject gfx;
    public GameObject[] levels;

    public float spinSpeed = 500f;

    [Header("Area Size")]
    [HideInInspector]
    public float areaSizeMultiplier = 1f;

    private Vector2 targetPosition;
    private Transform playerTransform;
    private Hammer owner;

    private bool returning = false;

    private Vector2 direction;

    private int level;

    private Vector3 originalScale;

    public void Initialize(
        Vector2 targetPosition,
        Transform playerTransform,
        Hammer owner,
        int level,
        float areaSizeMultiplier
    )
    {
        this.targetPosition = targetPosition;
        this.playerTransform = playerTransform;
        this.owner = owner;
        this.level = level;
        this.areaSizeMultiplier = areaSizeMultiplier;

        // Store original prefab scale
        originalScale =
            transform.localScale;

        // Apply Enchanted Lens
        transform.localScale =
            originalScale *
            areaSizeMultiplier;

        // Select the correct hammer visual
        if (levels != null && levels.Length > 0)
        {
            int index =
                Mathf.Clamp(
                    level - 1,
                    0,
                    levels.Length - 1
                );

            // Disable all levels
            for (int i = 0; i < levels.Length; i++)
            {
                if (levels[i] != null)
                {
                    levels[i].SetActive(false);
                }
            }

            // Enable current level
            if (levels[index] != null)
            {
                levels[index].SetActive(true);
            }
        }

        // Initial direction toward target
        direction =
            (
                targetPosition -
                (Vector2)transform.position
            ).normalized;

        StartCoroutine(
            ReturnHammer()
        );
    }

    void Update()
    {
        if (playerTransform == null)
            return;

        // Once returning, constantly update
        // direction toward the player
        if (returning)
        {
            direction =
                (
                    (Vector2)playerTransform.position -
                    (Vector2)transform.position
                ).normalized;
        }

        transform.Translate(
            direction *
            speed *
            Time.deltaTime
        );

        // Hammer has reached player
        if (
            returning &&
            Vector2.Distance(
                transform.position,
                playerTransform.position
            ) < 0.2f
        )
        {
            if (owner != null)
            {
                owner.HammerReturned();
            }

            Destroy(gameObject);
        }
    }

    IEnumerator ReturnHammer()
    {
        yield return new WaitForSeconds(
            returnDelay
        );

        returning = true;
    }

    void FixedUpdate()
    {
        if (gfx != null)
        {
            gfx.transform.Rotate(
                0f,
                0f,
                spinSpeed *
                Time.deltaTime
            );
        }
    }
}