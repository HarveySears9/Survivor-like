using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FireBreath : MonoBehaviour
{
    public GameObject fireballPrefab; // Reference to the Fireball prefab
    public float fireRate = 5f;       // Time between each fire breath
    public int level = 1;             // Weapon level (determines number of fireballs)
    public float spreadAngle = 30f;   // Total spread angle for the fireballs
    private float nextFireTime = 0f;  // Tracks when the next fire is allowed

    public Vector2 moveDirection; // The direction of the player’s movement

    void Update()
    {
        if (Time.time >= nextFireTime && moveDirection != Vector2.zero)
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate; // Set next fire time
        }
    }

    void Fire()
    {
        int fireballCount = level + 2; // More fireballs as the level increases
        float angleStep = spreadAngle / (fireballCount - 1);
        float startAngle = -spreadAngle / 2;

        for (int i = 0; i < fireballCount; i++)
        {
            // Calculate the fireball's direction
            float angle = startAngle + i * angleStep;
            Vector2 fireDirection = Quaternion.Euler(0, 0, angle) * moveDirection;

            // Instantiate the fireball
            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);

            // Set the rotation to match the fireball's movement direction
            float angleToRotate = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
            fireball.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleToRotate));

            // Pass the fire direction to the fireball script
            fireball.GetComponent<Fireball>().Initialize(Vector2.right);
        }
    }

    public void Ability()
    {
        StartCoroutine(BurstFire());
    }

    IEnumerator BurstFire()
    {
        // Store the original fire rate
        float originalFR = fireRate;

        // Temporarily set the fire rate to a faster value
        fireRate = 10f;

        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Restore the original fire rate
        fireRate = originalFR;
    }

}
