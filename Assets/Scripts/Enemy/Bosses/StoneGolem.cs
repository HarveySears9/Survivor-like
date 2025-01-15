using System.Collections;
using UnityEngine;

public class StoneGolem : Boss
{
    public GameObject boulderPrefab; // Reference to the boulder prefab
    public float boulderCooldown = 5f;   // Time between boulder attacks

    public int boulderCount = 3; // Number of boulders to throw
    public float spreadAngle = 30f; // Total spread angle for the boulders
    public float boulderDamage = 1f;

    private float nextBoulderTime;

    protected override void Start()
    {
        base.Start();
        // Initialize additional Stone Golem-specific logic
        nextBoulderTime = Time.time + boulderCooldown;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        PerformStoneGolemAbilities();
    }

    private void PerformStoneGolemAbilities()
    {
        // Boulder Attack
        if (Time.time >= nextBoulderTime)
        {
            StartCoroutine(ThrowBoulder());
            nextBoulderTime = Time.time + boulderCooldown;
        }
    }

    private IEnumerator ThrowBoulder()
    {
        moving = false; // Stop movement while performing the ability

        yield return new WaitForSeconds(0.25f); // Optional delay before throwing the boulders

        if (boulderPrefab == null || playerTransform == null)
        {
            Debug.LogWarning("BoulderPrefab or PlayerTransform is missing!");
            yield break;
        }

        float angleStep = spreadAngle / (boulderCount - 1); // Step between each boulder
        float startAngle = -spreadAngle / 2; // Starting angle for the spread

        for (int i = 0; i < boulderCount; i++)
        {
            // Calculate the direction for this boulder
            float angle = startAngle + i * angleStep;
            Vector2 fireDirection = Quaternion.Euler(0, 0, angle) * (playerTransform.position - transform.position).normalized;

            // Instantiate the boulder
            GameObject boulder = Instantiate(boulderPrefab, transform.position, Quaternion.identity);

            // Set the rotation to match the boulder's movement direction
            float angleToRotate = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
            boulder.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleToRotate));

            // Pass the fire direction to the boulder's script
            Boulder boulderScript = boulder.GetComponent<Boulder>();
            if (boulderScript != null)
            {
                boulderScript.Initialize(Vector2.right);
                boulderScript.damage = boulderDamage;
            }

            yield return new WaitForSeconds(0.1f); // Slight delay between throwing each boulder
        }

        yield return new WaitForSeconds(0.5f); // Optional delay after the ability before resuming movement
        moving = true;
    }
}
