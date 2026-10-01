using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class DamageOverTimeArea : MonoBehaviour
{
    [Header("DoT Settings")]
    public float damagePerSecond = 1f;

    public bool damageOnEnter = true;

    private HashSet<EnemyController> enemiesInRange =
        new HashSet<EnemyController>();

    public void SetDamage(float damage)
    {
        damagePerSecond = damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy =
                other.GetComponent<EnemyController>();

            if (enemy != null)
            {
                enemiesInRange.Add(enemy);

                if (damageOnEnter)
                {
                    enemy.TakeDamage(
                        damagePerSecond *
                        Time.fixedDeltaTime
                    );
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy =
                other.GetComponent<EnemyController>();

            if (enemy != null)
            {
                enemiesInRange.Remove(enemy);
            }
        }
    }

    private void FixedUpdate()
    {
        float tickDamage =
            damagePerSecond *
            Time.fixedDeltaTime;

        EnemyController[] enemiesArray =
            new EnemyController[
                enemiesInRange.Count
            ];

        enemiesInRange.CopyTo(enemiesArray);

        foreach (EnemyController enemy in enemiesArray)
        {
            if (enemy != null)
            {
                enemy.TakeDamage(tickDamage);
            }
        }
    }
}