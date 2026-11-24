using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public sealed class PoolManager : MonoBehaviour
{
    [Serializable]
    private sealed class PoolEntry
    {
        public PoolableObject prefab;
        [Min(1)] public int capacity = 1;
    }

    [SerializeField] private PoolEntry[] entries = Array.Empty<PoolEntry>();

    private readonly Dictionary<PoolableObject, ObjectPool<PoolableObject>> pools =
        new Dictionary<PoolableObject, ObjectPool<PoolableObject>>();

    private Transform inactiveObjects;

    private void Awake()
    {
        GameObject container = new GameObject(ObjectNames.Inactive);
        container.SetActive(false);
        container.transform.SetParent(transform, false);
        inactiveObjects = container.transform;

        foreach (PoolEntry entry in entries)
        {
            if (entry.prefab == null || entry.capacity < 1)
            {
                throw new InvalidOperationException(
                    $"Invalid pool entry on {name}. Assign a prefab and a positive capacity.");
            }

            pools.Add(entry.prefab, CreatePool(entry.prefab, entry.capacity));
        }
    }

    public T Get<T>(T prefab) where T : PoolableObject
    {
        if (prefab == null)
            throw new ArgumentNullException(nameof(prefab));

        if (!pools.TryGetValue(prefab, out ObjectPool<PoolableObject> pool))
        {
            throw new ArgumentException(
                $"No pool is configured for {prefab.name} on {name}.",
                nameof(prefab));
        }

        PoolableObject instance = pool.Get();
        instance.SetPool(pool);
        return (T)instance;
    }

    private ObjectPool<PoolableObject> CreatePool(
        PoolableObject prefab, int capacity)
    {
        ObjectPool<PoolableObject> pool = new ObjectPool<PoolableObject>(
            createFunc: () =>
            {
                PoolableObject instance =
                    Instantiate(prefab, inactiveObjects, false);

                instance.gameObject.SetActive(false);
                return instance;
            },
            actionOnGet: instance =>
                instance.transform.SetParent(null, false),
            actionOnRelease: instance =>
            {
                instance.gameObject.SetActive(false);
                instance.transform.SetParent(inactiveObjects, false);
            },
            actionOnDestroy: instance => Destroy(instance.gameObject),
            defaultCapacity: capacity,
            maxSize: capacity);

        Prewarm(pool, capacity);
        return pool;
    }

    private void Prewarm(ObjectPool<PoolableObject> pool, int count)
    {
        PoolableObject[] instances = new PoolableObject[count];

        for (int i = 0; i < count; i++)
            instances[i] = pool.Get();

        foreach (PoolableObject instance in instances)
            pool.Release(instance);
    }

    private void OnDestroy()
    {
        foreach (ObjectPool<PoolableObject> pool in pools.Values)
            pool.Dispose();
    }
}
