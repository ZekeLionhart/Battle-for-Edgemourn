using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public sealed class EnemyPool : MonoBehaviour
{
    [SerializeField] private EnemyBase gruntPrefab;
    [SerializeField, Min(1)] private int maxInactiveGrunts = 20;
    [SerializeField] private EnemyBase shielderPrefab;
    [SerializeField, Min(1)] private int maxInactiveShielders = 15;
    [SerializeField] private EnemyBase ramPrefab;
    [SerializeField, Min(1)] private int maxInactiveRams = 3;
    [SerializeField] private EnemyBase trebuchetPrefab;
    [SerializeField, Min(1)] private int maxInactiveTrebuchets = 3;

    private readonly Dictionary<EnemyBase, ObjectPool<EnemyBase>> enemyPools = new();

    private Transform inactiveEnemies;

    private void Awake()
    {
        GameObject container = new(ObjectNames.EnemyPool);
        container.SetActive(false);
        container.transform.SetParent(transform, false);
        inactiveEnemies = container.transform;

        enemyPools.Add(gruntPrefab, CreatePool(gruntPrefab, maxInactiveGrunts));
        enemyPools.Add(shielderPrefab, CreatePool(shielderPrefab, maxInactiveShielders));
        enemyPools.Add(ramPrefab, CreatePool(ramPrefab, maxInactiveRams));
        enemyPools.Add(trebuchetPrefab, CreatePool(trebuchetPrefab, maxInactiveTrebuchets));
    }

    public EnemyBase GetEnemy(EnemyBase prefab)
    {
        if (!enemyPools.TryGetValue(prefab, out ObjectPool<EnemyBase> pool))
        {
            throw new System.ArgumentException(
                "The enemy prefab has no configured pool.",
                nameof(prefab));
        }

        EnemyBase enemy = pool.Get();
        enemy.SetPool(pool);
        return enemy;
    }

    private ObjectPool<EnemyBase> CreatePool(EnemyBase prefab, int maxInactive)
    {
        ObjectPool<EnemyBase> pool = new ObjectPool<EnemyBase>(
            createFunc: () =>
            {
                EnemyBase instance = Instantiate(prefab, inactiveEnemies, false);
                instance.gameObject.SetActive(false);
                return instance;
            },
            actionOnGet: instance => instance.transform.SetParent(null, false),
            actionOnRelease: instance =>
            {
                instance.gameObject.SetActive(false);
                instance.transform.SetParent(inactiveEnemies, false);
            },
            actionOnDestroy: instance => Destroy(instance.gameObject),
            maxSize: maxInactive);

        Prewarm(pool, maxInactive);
        return pool;
    }

    private void Prewarm(ObjectPool<EnemyBase> pool, int count)
    {
        EnemyBase[] instances = new EnemyBase[count];

        for (int i = 0; i < count; i++)
            instances[i] = pool.Get();

        foreach (EnemyBase instance in instances)
            pool.Release(instance);
    }

    private void OnDestroy()
    {
        foreach (ObjectPool<EnemyBase> pool in enemyPools.Values)
            pool.Dispose();
    }
}
