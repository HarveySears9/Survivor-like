using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool Instance;

    [SerializeField] private int defaultPoolSize = 30;

    private Dictionary<GameObject, Queue<GameObject>> pools =
        new Dictionary<GameObject, Queue<GameObject>>();

    void Awake()
    {
        Instance = this;
    }

    public GameObject Get(
        GameObject prefab,
        Vector3 position,
        Transform player)
    {
        if (prefab == null)
            return null;

        // Create a pool for this prefab if one doesn't exist yet
        if (!pools.TryGetValue(prefab, out Queue<GameObject> pool))
        {
            pool = new Queue<GameObject>();
            pools.Add(prefab, pool);

            for (int i = 0; i < defaultPoolSize; i++)
            {
                GameObject newEnemy = CreateNewObject(prefab);
                pool.Enqueue(newEnemy);
            }
        }

        // Get an existing enemy, or create a new one if the pool is empty
        GameObject enemy;

        if (pool.Count > 0)
        {
            enemy = pool.Dequeue();
        }
        else
        {
            enemy = CreateNewObject(prefab);
        }

        // Set position BEFORE enabling the enemy
        enemy.transform.SetPositionAndRotation(
    position,
    Quaternion.identity
);

        EnemyController controller =
            enemy.GetComponent<EnemyController>();

        if (controller != null)
        {
            controller.playerTransform = player;
            controller.ResetEnemy();
        }

        // Make absolutely sure the pooled enemy is at the requested position
        enemy.transform.SetPositionAndRotation(
            position,
            Quaternion.identity
        );

        enemy.SetActive(true);

        return enemy;
    }

    public void Return(GameObject enemy)
    {
        if (enemy == null)
            return;

        PooledEnemy pooledEnemy =
            enemy.GetComponent<PooledEnemy>();

        if (pooledEnemy == null ||
            pooledEnemy.prefab == null)
        {
            Destroy(enemy);
            return;
        }

        enemy.SetActive(false);

        pools[pooledEnemy.prefab].Enqueue(enemy);
    }

    private GameObject CreateNewObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);

        PooledEnemy pooledEnemy =
            obj.GetComponent<PooledEnemy>();

        if (pooledEnemy == null)
        {
            pooledEnemy = obj.AddComponent<PooledEnemy>();
        }

        pooledEnemy.prefab = prefab;

        // Always create pooled enemies inactive
        obj.SetActive(false);

        return obj;
    }
}