using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShot : MonoBehaviour
{
    public GameObject arrowPrefab; // Reference to the Fireball prefab
    public float fireRate = 1f;       // Time between each fire breath
    public int level = 0;             // Weapon level (determines number of fireballs)
    public int maxLevel = 5;
    public float range = 10f;         // Range to detect targets
    private float nextFireTime = 0f;  // Tracks when the next fire is allowed

    public AnimateSprite playerAnimator;
    public AnimateImage levelUpButtonAnimator;

    public Sprite[] bow1, bow1moving, bow2, bow2moving, bow3, bow3moving, bow4, bow4moving, bow5, bow5moving;

    public LevelUpButtons levelUpButton;

    public SpriteRenderer sr;

    public PlayerController player;

    private float damage;

    void Start()
    {
        SaveFile.Data loadedData = SaveFile.LoadData<SaveFile.Data>();

        // Check if the current character is not Kaelira
        if (loadedData.currentCharacter != 1)
        {
            // Deactivate the FireBreath GameObject
            this.gameObject.SetActive(false);
            return; // Exit early since FireBreath should not be initialized
        }
        damage = SaveFile.LoadData<SaveFile.Data>().currentDamage;
        levelUpButton.LevelUp(level, maxLevel);
        UpdateSprites();
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            FireAtTargets();
            nextFireTime = Time.time + 1f / fireRate; // Set next fire time
        }

        playerAnimator.isMoving = player.isMoving;

        // Flip the sprite's X-axis based on the player's movement direction
        if (player.moveDirection.x < 0)
        {
            sr.flipX = true; // Flip sprite when moving left
        }
        else if (player.moveDirection.x > 0)
        {
            sr.flipX = false; // Keep sprite normal when moving right
        }
    }

    void FireAtTargets()
    {
        StartCoroutine(FireBurst());
    }

    IEnumerator FireBurst()
    {
        for (int i = 0; i < level; i++)
        {
            Transform target = FindTargets();

            if (target == null) break; // Exit if no targets are found

            // Calculate direction to the target
            Vector2 fireDirection = (target.position - transform.position).normalized;

            // Instantiate the fireball
            GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);

            // Set the rotation to match the fireball's movement direction
            // Add a random variation to the angle
            float randomOffset = Random.Range(-5f, 5f); // Adjust the range to control the spread (e.g., -5 to 5 degrees)
            float angleToRotate = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg + randomOffset;
            arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleToRotate));

            // Pass the fire direction to the fireball script
            arrow.GetComponent<Fireball>().Initialize(Vector2.right);
            arrow.GetComponent<Weapon>().damage = damage;

            yield return new WaitForSeconds(0.1f);
        }
    }

    private Transform FindTargets()
    {
        List<Transform> enemyTargets = new List<Transform>();

        // Find all colliders within the range
        Collider2D[] targetsInRange = Physics2D.OverlapCircleAll(transform.position, range);

        // Filter only enemy targets
        foreach (var target in targetsInRange)
        {
            if (target.CompareTag("Enemy")) // Make sure your enemies have the "Enemy" tag
            {
                enemyTargets.Add(target.transform);
            }
        }

        if (enemyTargets.Count == 0) return null; // Exit if no targets are found

        // Find the closest target
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform enemy in enemyTargets)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestTarget = enemy;
            }
        }

        return closestTarget; // Return the closest enemy
    }

    public void LevelUp()
    {
        level++;
        if (level > maxLevel)
        {
            level = maxLevel;
        }

        levelUpButton.LevelUp(level, maxLevel);

        UpdateSprites();
    }

    void UpdateSprites()
    {
        switch (level)
        {
            case 1:
                playerAnimator.spriteArray = bow1;
                playerAnimator.moveArray = bow1moving;
                levelUpButtonAnimator.spriteArray = bow2;
                break;
            case 2:
                playerAnimator.spriteArray = bow2;
                playerAnimator.moveArray = bow2moving;
                levelUpButtonAnimator.spriteArray = bow3;
                break;
            case 3:
                playerAnimator.spriteArray = bow3;
                playerAnimator.moveArray = bow3moving;
                levelUpButtonAnimator.spriteArray = bow4;
                break;
            case 4:
                playerAnimator.spriteArray = bow4;
                playerAnimator.moveArray = bow4moving;
                levelUpButtonAnimator.spriteArray = bow5;
                break;
            case 5:
                playerAnimator.spriteArray = bow5;
                playerAnimator.moveArray = bow5moving;
                break;
        }
    }
}
