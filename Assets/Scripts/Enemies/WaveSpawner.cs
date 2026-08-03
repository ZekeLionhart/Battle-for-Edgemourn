using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private List<Wave> waves;

    private void Start()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        foreach (Wave wave in waves)
        {
            yield return new WaitForSeconds(wave.startDelay);

            for (int i = 0; i < wave.amount; i++)
            {
                Instantiate(wave.enemy, transform.position, transform.rotation);

                if (i < wave.amount - 1)
                    yield return new WaitForSeconds(wave.interval);
            }
        }
    }
}
