using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBomb : MonoBehaviour
{
    public float explosionRadius = 3f; // Radius of the AoE
    public float slowDuration = 2f;   // Duration enemies are slowed
    public float slowAmount = 0.5f;   // How much to slow enemies (50% speed)
    public float detonationTime = 2f; // Time before the bomb explodes
    public GameObject explosionEffect; // Particle effect for the explosion

    void Start()
    {
        // Start the detonation timer
        StartCoroutine(Detonate());
    }

    IEnumerator Detonate()
    {
        // Wait for the set detonation time
        yield return new WaitForSeconds(detonationTime);

        // Trigger the explosion
        Explode();
    }

    void Explode()
    {
        // Instantiate explosion effect
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Get all colliders in the explosion radius
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hitColliders)
        {
            if (hit.CompareTag("Enemy"))
            {
                // Apply slowing effect to enemies
                EnemyController enemy = hit.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.ApplySlow(slowAmount, slowDuration);
                }
            }
        }

        // Destroy the bomb object
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Draw the explosion radius in the editor for debugging
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
