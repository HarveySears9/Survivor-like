using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : MonoBehaviour
{
    public PlayerController player;
    public PenguinSummoner summoner;

    [Header("Ice Pulse")]
    public GameObject icePulsePrefab;

    public float moveSpeed = 3f;
    public float followDistance = 2f;

    public float range = 10f;

    private int level = 1;

    private AnimateSprite animateSprite;
    private SpriteRenderer sr;

    private Transform target;

    private bool returningToPlayer = false;

    private bool attacking = false;

    public float[] pulseScales = { 1f, 1.2f, 1.4f, 1.6f, 1.8f };
    private Vector3 originalPulseScale;

    void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }

        if (summoner == null)
        {
            summoner = FindObjectOfType<PenguinSummoner>();
        }

        animateSprite = GetComponent<AnimateSprite>();
        sr = GetComponent<SpriteRenderer>();

        originalPulseScale = icePulsePrefab.transform.localScale;
    }


    void Update()
    {
        if (!attacking)
        {
            FollowPlayer();
            return;
        }

        if (returningToPlayer)
        {
            FollowPlayer();
            return;
        }

        if (target == null)
        {
            FindTarget();
        }

        if (target != null)
        {
            RunTowardsTarget();
        }
    }

    public void StartAttack()
    {
        attacking = true;
    }

    private void FollowPlayer()
    {
        if (player == null)
            return;

        Vector3 targetPosition = player.transform.position;

        float distance = Vector2.Distance(
            transform.position,
            targetPosition
        );

        // Only move if we're too far away
        if (distance > followDistance)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
            animateSprite.isMoving = true;

            // Flip sprite when moving left
            if (targetPosition.x < transform.position.x)
                sr.flipX = true;
            else if (targetPosition.x > transform.position.x)
                sr.flipX = false;
        }
        else
        {
            animateSprite.isMoving = false;

            if (returningToPlayer)
            {
                returningToPlayer = false;
                attacking = false;

                summoner.StartCooldown();
            }
        }
    }

    private void RunTowardsTarget()
    {
        if (target == null)
            return;

        float distance = Vector2.Distance(
            transform.position,
            target.position
        );

        if (distance > 1f)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                target.position,
                moveSpeed * Time.deltaTime * 1.25f
            );

            animateSprite.isMoving = true;

            if (target.position.x < transform.position.x)
                sr.flipX = true;
            else if (target.position.x > transform.position.x)
                sr.flipX = false;
        }
        else
        {
            // Spawn the ice pulse at the Penguin's position
            if (icePulsePrefab != null)
            {
                GameObject pulse = Instantiate(
                    icePulsePrefab,
                    transform.position,
                    Quaternion.identity
                );

                pulse.transform.localScale = originalPulseScale * pulseScales[level - 1];
            }

            target = null;
            returningToPlayer = true;
            animateSprite.isMoving = false;
        }
    }

    private void FindTarget()
    {
        Collider2D[] targetsInRange = Physics2D.OverlapCircleAll(
            transform.position,
            range
        );

        List<Transform> enemyTargets = new List<Transform>();

        foreach (var col in targetsInRange)
        {
            if (col.CompareTag("Enemy"))
            {
                enemyTargets.Add(col.transform);
            }
        }

        if (enemyTargets.Count == 0)
        {
            target = null;
            return;
        }

        target = enemyTargets[0];
    }


    public void SetLevel(int newLevel)
    {
        level = newLevel;
    }
}