using System.Collections;
using UnityEngine;

public class UnlimitedSpawner : MonoBehaviour
{
    [SerializeField] private EnemyBase enemy;
    [SerializeField] private int squadSize;
    [SerializeField] private float distBetweenEach;
    [SerializeField] private float enemyDelay;
    [SerializeField] private EnemyPool enemyPool;
    [SerializeField] private ProjectilePool projectilePool;

    private WaitForSeconds enemyWFS;

    private void Awake()
    {
        enemyWFS = new WaitForSeconds(enemyDelay);
    }

    private void OnEnable()
    {
        GameManager.OnGameEnded += StopSpawning;
    }

    private void OnDisable()
    {
        GameManager.OnGameEnded -= StopSpawning;
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private EnemyBase CreateEnemy(EnemyBase prefab, Vector3 position)
    {
        EnemyBase enemy = enemyPool.GetEnemy(prefab);
        enemy.transform.SetPositionAndRotation(position, transform.rotation);

        if (enemy is TrebuchetManager trebuchet)
            trebuchet.SetProjectilePool(projectilePool);

        enemy.SpawnEnemy();
        return enemy;
    }

    private IEnumerator SpawnEnemy()
    {
        yield return enemyWFS;

        for (int i = 0; i < squadSize; i++)
            CreateEnemy(enemy, transform.position + distBetweenEach * i * Vector3.right);

        StartCoroutine(SpawnEnemy());
    }

    private void StopSpawning()
    {
        StopAllCoroutines();
    }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    public void DebugSpawnEnemy(EnemyBase prefab)
    {
        CreateEnemy(prefab, transform.position);
    }
#endif
}
