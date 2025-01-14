using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwordSlash : MonoBehaviour
{
    public GameObject slashPrefab; // Reference to the Fireball prefab
    public float fireRate = 5f;       // Time between each fire breath
    public int level = 0;             // Weapon level (determines number of fireballs)
    public int maxLevel = 5;
    public float range = 10f;         // Range to detect targets
    private float nextFireTime = 0f;  // Tracks when the next fire is allowed

    public AnimateSprite playerAnimator;
    public AnimateImage levelUpButtonAnimator;

    public Sprite[] sword1 ,sword1moving, sword2, sword2moving, sword3, sword3moving, sword4, sword4moving, sword5, sword5moving;

    public LevelUpButtons levelUpButton;

    void Start()
    {
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
    }

    void FireAtTargets()
    {
        // Find all colliders within the range
        Collider2D[] targetsInRange = Physics2D.OverlapCircleAll(transform.position, range);

        // Filter only enemy targets
        List<Transform> enemyTargets = new List<Transform>();
        foreach (var target in targetsInRange)
        {
            if (target.CompareTag("Enemy")) // Make sure your enemies have the "Enemy" tag
            {
                enemyTargets.Add(target.transform);
            }
        }

        if (enemyTargets.Count == 0) return; // Exit if no targets are found

        for (int i = 0; i < level; i++)
        {
            Transform target = enemyTargets[i % enemyTargets.Count]; // Cycle through targets if there are fewer than fireballs

            // Calculate direction to the target
            Vector2 fireDirection = (target.position - transform.position).normalized;

            // Instantiate the fireball
            GameObject slash = Instantiate(slashPrefab, transform.position, Quaternion.identity);

            // Set the rotation to match the fireball's movement direction
            // Add a random variation to the angle
            float randomOffset = Random.Range(-5f, 5f); // Adjust the range to control the spread (e.g., -5 to 5 degrees)
            float angleToRotate = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg + randomOffset;
            slash.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleToRotate));

            // Pass the fire direction to the fireball script
            slash.GetComponent<Fireball>().Initialize(Vector2.right);
        }
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
        switch(level)
        {
            case 1:
                playerAnimator.spriteArray = sword1;
                playerAnimator.moveArray = sword1moving;
                levelUpButtonAnimator.spriteArray = sword2;
                break;
            case 2:
                playerAnimator.spriteArray = sword2;
                playerAnimator.moveArray = sword2moving;
                levelUpButtonAnimator.spriteArray = sword3;
                break;
            case 3:
                playerAnimator.spriteArray = sword3;
                playerAnimator.moveArray = sword3moving;
                levelUpButtonAnimator.spriteArray = sword4;
                break;
            case 4:
                playerAnimator.spriteArray = sword4;
                playerAnimator.moveArray = sword4moving;
                levelUpButtonAnimator.spriteArray = sword5;
                break;
            case 5:
                playerAnimator.spriteArray = sword5;
                playerAnimator.moveArray = sword5moving;
                break;
        }
    }
}
