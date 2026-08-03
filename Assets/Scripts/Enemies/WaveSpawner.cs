using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private List<Wave> waves;
    private WaitForSeconds waveWFS;

    public static Action<EnemyBase> OnEnemySpawn;
    public static Action OnFinishedSpawning;

    private void Awake()
    {
        waveWFS = new WaitForSeconds(0f);
    }

    private void Start()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        foreach (Wave wave in waves)
        {
            waveWFS = new WaitForSeconds(wave.interval);

            for (int i = 0; i < wave.amount; i++)
            {
                if (!wave.IsDelayOnly)
                    OnEnemySpawn(Instantiate(wave.enemy, transform.position, transform.rotation));

                if (i < wave.amount - 1)
                    yield return waveWFS;
            }

            yield return new WaitForSeconds(wave.nextWaveDelay);
        }

        OnFinishedSpawning();
    }
}
