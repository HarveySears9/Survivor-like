using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    public static ItemPool Instance;

    [SerializeField] private int defaultPoolSize = 30;

    private Dictionary<GameObject, Queue<GameObject>> pools =
        new Dictionary<GameObject, Queue<GameObject>>();

    void Awake()
    {
        Instance = this;
    }

    public GameObject Get(GameObject prefab)
    {
        if (prefab == null)
            return null;

        if (!pools.TryGetValue(prefab, out Queue<GameObject> pool))
        {
            pool = new Queue<GameObject>();
            pools.Add(prefab, pool);

            for (int i = 0; i < defaultPoolSize; i++)
            {
                CreateNewObject(prefab, pool);
            }
        }

        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        return CreateNewObject(prefab, pool, true);
    }

    public void Return(GameObject obj)
    {
        if (obj == null)
            return;

        PooledItem pooledItem = obj.GetComponent<PooledItem>();

        if (pooledItem == null || pooledItem.prefab == null)
        {
            Destroy(obj);
            return;
        }

        obj.SetActive(false);

        pools[pooledItem.prefab].Enqueue(obj);
    }

    GameObject CreateNewObject(
        GameObject prefab,
        Queue<GameObject> pool,
        bool activate = false)
    {
        GameObject obj = Instantiate(prefab);

        PooledItem pooledItem = obj.GetComponent<PooledItem>();

        if (pooledItem == null)
            pooledItem = obj.AddComponent<PooledItem>();

        pooledItem.prefab = prefab;

        obj.SetActive(activate);

        if (!activate)
            pool.Enqueue(obj);

        return obj;
    }
}