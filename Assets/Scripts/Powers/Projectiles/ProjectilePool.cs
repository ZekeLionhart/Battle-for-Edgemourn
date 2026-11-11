using UnityEngine;
using UnityEngine.Pool;

public sealed class ProjectilePool : MonoBehaviour
{
    [SerializeField] private ArrowManager arrowPrefab;
    [SerializeField, Min(1)] private int maxInactiveArrows = 50;
    [SerializeField] private FireballManager fireballPrefab;
    [SerializeField, Min(1)] private int maxInactiveFireballs = 1;
    [SerializeField] private ExplosionManager explosionPrefab;
    [SerializeField, Min(1)] private int maxInactiveExplosions = 1;
    [SerializeField] private ExplosionManager lightningPrefab;
    [SerializeField, Min(1)] private int maxInactiveBolts = 1;
    [SerializeField] private WallManager stonewallPrefab;
    [SerializeField, Min(1)] private int maxInactiveWalls = 1;
    [SerializeField] private ProjectileManager tarPrefab;
    [SerializeField, Min(1)] private int maxInactiveTars = 1;

    private ObjectPool<ArrowManager> arrowPool;
    private ObjectPool<FireballManager> fireballPool;
    private ObjectPool<ExplosionManager> explosionPool;
    private ObjectPool<ExplosionManager> lightningPool;
    private ObjectPool<WallManager> stonewallPool;
    private ObjectPool<ProjectileManager> tarPool;

    private Transform inactiveProjectiles;

    private void Awake()
    {
        GameObject container = new("Inactive Projectiles"); // string
        container.SetActive(false);
        container.transform.SetParent(transform, false);
        inactiveProjectiles = container.transform;

        arrowPool = CreatePool(arrowPrefab, maxInactiveArrows);
        fireballPool = CreatePool(fireballPrefab, maxInactiveFireballs);
        explosionPool = CreatePool(explosionPrefab, maxInactiveExplosions);
        lightningPool = CreatePool(lightningPrefab, maxInactiveBolts);
    }

    public ArrowManager GetArrow()
    {
        ArrowManager arrow = arrowPool.Get();
        arrow.SetPool(arrowPool);
        return arrow;
    }

    public FireballManager GetFireball()
    {
        FireballManager fireball = fireballPool.Get();
        fireball.SetPool(fireballPool);
        return fireball;
    }

    public ExplosionManager GetExplosion()
    {
        ExplosionManager explosion = explosionPool.Get();
        explosion.SetPool(explosionPool);
        return explosion;
    }

    public ExplosionManager GetLightning()
    {
        ExplosionManager lightning = lightningPool.Get();
        lightning.SetPool(lightningPool);
        return lightning;
    }

    private ObjectPool<T> CreatePool<T>(T prefab, int maxInactive) where T : ProjectileManager
    {
        ObjectPool<T> pool = new ObjectPool<T>(
            createFunc: () =>
            {
                T instance = Instantiate(prefab, inactiveProjectiles, false);
                instance.gameObject.SetActive(false);
                return instance;
            },
            actionOnGet: instance => instance.transform.SetParent(null, false),
            actionOnRelease: instance =>
            {
                instance.gameObject.SetActive(false);
                instance.transform.SetParent(inactiveProjectiles, false);
            },
            actionOnDestroy: instance => Destroy(instance.gameObject),
            maxSize: maxInactive);

        Prewarm(pool, maxInactive);
        return pool;
    }

    private void Prewarm<T>(ObjectPool<T> pool, int count) where T : class
    {
        T[] instances = new T[count];

        for (int i = 0; i < count; i++)
            instances[i] = pool.Get();

        foreach (T instance in instances)
            pool.Release(instance);
    }

    private void OnDestroy()
    {
        arrowPool.Dispose();
        fireballPool.Dispose();
        explosionPool.Dispose();
        lightningPool.Dispose();
    }
}
