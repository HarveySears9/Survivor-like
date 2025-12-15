using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.GetComponent<EnemyController>();
            Boss boss = collision.GetComponent<Boss>();
            if (enemy != null)
            {
                enemy.ApplySlow(0.4f, 2f); // 40% slow for 2 seconds
            }
            else if(boss != null)
            {
                boss.ApplySlow(0.4f, 2f);
            }

            Destroy(gameObject); // Or pool it if you use pooling
        }
    }

}
