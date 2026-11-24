using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private List<Wave> waves;
    [SerializeField] private PoolManager enemyPool;
    [SerializeField] private PoolManager enemyProjecPool;
    [SerializeField] private PoolManager vfxPool;

    private WaitForSeconds waveWFS;

    public static event Action<EnemyBase> OnEnemySpawn;
    public static event Action OnFinishedSpawning;

    private void Awake()
    {
        waveWFS = new WaitForSeconds(0f);
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
        StartCoroutine(SpawnWave());
    }

    private EnemyBase CreateEnemy(EnemyBase prefab, Vector3 position)
    {
        EnemyBase enemy = enemyPool.Get(prefab);
        enemy.transform.SetPositionAndRotation(position, transform.rotation);
        enemy.SetVFXPool(vfxPool);

        if (enemy is TrebuchetManager trebuchet)
            trebuchet.SetProjectilePool(enemyProjecPool);

        enemy.SpawnEnemy();
        return enemy;
    }

    private IEnumerator SpawnWave()
    {
        foreach (Wave wave in waves)
        {
            waveWFS = new WaitForSeconds(wave.interval);

            for (int i = 0; i < wave.amount; i++)
            {
                if (!wave.IsDelayOnly)
                    OnEnemySpawn(CreateEnemy(wave.enemy, transform.position));

                if (i < wave.amount - 1)
                    yield return waveWFS;
            }

            yield return new WaitForSeconds(wave.nextWaveDelay);
        }

        OnFinishedSpawning();
    }

    private void StopSpawning()
    {
        StopAllCoroutines();
    }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    public void DebugSpawnEnemy(EnemyBase prefab)
    {
        OnEnemySpawn(CreateEnemy(prefab, transform.position));
    }
#endif
}
