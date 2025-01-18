using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireBreath : MonoBehaviour
{
    public GameObject fireballPrefab; // Reference to the Fireball prefab
    public float fireRate = 5f;       // Time between each fire breath
    public int level = 1;             // Weapon level (determines number of fireballs)
    public int maxLevel = 5;
    public float spreadAngle = 30f;   // Total spread angle for the fireballs
    public float range = 10f;   // Total spread angle for the fireballs
    private float nextFireTime = 0f;  // Tracks when the next fire is allowed

    private bool aimWithJoystick = false;

    public LevelUpButtons levelUpButton;

    public Vector2 moveDirection = Vector2.right; // The direction of the player’s movement

    private float damage;

    void Start()
    {
        damage = SaveFile.LoadData<SaveFile.Data>().currentDamage;
        levelUpButton.LevelUp(level, maxLevel);
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate; // Set next fire time
        }
    }

    void Fire()
    {
        //int fireballCount = (2*level) + 1; // More fireballs as the level increases
        int fireballCount = 3;
        float angleStep = spreadAngle / (fireballCount - 1);
        float startAngle = -spreadAngle / 2;

        Transform enemyTarget = FindTargets();

        Vector2 fireDirection;

        for (int i = 0; i < fireballCount; i++)
        {
            // Calculate the fireball's direction
            float angle = startAngle + i * angleStep;

            if (aimWithJoystick)
            {
                // Use joystick direction with spread
                fireDirection = Quaternion.Euler(0, 0, angle) * moveDirection;
            }
            else
            {
                if (enemyTarget == null) return; // Exit if no targets are found

                // Calculate direction to the target
                fireDirection = (enemyTarget.position - transform.position).normalized;

                // Add a spread angle to the direction
                fireDirection = Quaternion.Euler(0, 0, angle) * fireDirection;
            }
        


            // Instantiate the fireball
            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);

            // Set the rotation to match the fireball's movement direction
            float angleToRotate = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
            fireball.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleToRotate));

            // Pass the fire direction to the fireball script
            fireball.GetComponent<Fireball>().Initialize(Vector2.right);
            fireball.GetComponent<Weapon>().damage = damage;
        }
    }

    private Transform FindTargets()
    {
        List<Transform> enemyTargets = new List<Transform>();

        if (!aimWithJoystick)
        {
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
        else
        {
            return null; // No target if using joystick aiming
        }
    }


    public void Ability()
    {
        StartCoroutine(BurstFire());
    }

    public void LevelUp()
    {
        level++;
        if (level > maxLevel)
        {
            level = maxLevel;
        }

        levelUpButton.LevelUp(level, maxLevel);

        fireRate++;
    }

    IEnumerator BurstFire()
    {
        // Store the original fire rate
        float originalFR = fireRate;

        aimWithJoystick = true;

        // Temporarily set the fire rate to a faster value
        fireRate = 10f;

        nextFireTime = 0f;

        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Restore the original fire rate
        fireRate = originalFR;

        aimWithJoystick = false;
    }

}
