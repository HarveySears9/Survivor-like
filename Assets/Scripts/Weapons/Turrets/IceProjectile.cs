using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : MonoBehaviour
{

    public bool destroyOnHit = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
            return;

        EnemyController enemy = collision.GetComponent<EnemyController>();
        Boss boss = collision.GetComponent<Boss>();

        if (enemy != null)
        {
            enemy.ApplySlow(0.4f, 2f);
        }
        else if (boss != null)
        {
            boss.ApplySlow(0.4f, 2f);
        }

        if (destroyOnHit)
            Destroy(gameObject);
    }

}
