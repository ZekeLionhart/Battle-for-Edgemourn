using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private GameObject enemy;
    [SerializeField] private float enemyDelay;
    private WaitForSeconds enemyDelayWFS;
    private bool canSpawnEnemy = true;

    private void Awake()
    {
        enemyDelayWFS = new WaitForSeconds(enemyDelay);
    }

    private void Update()
    {
        if (canSpawnEnemy)
            StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        canSpawnEnemy = false;

        yield return enemyDelayWFS;

        Instantiate(enemy, enemySpawnPoint.position, Quaternion.identity);

        canSpawnEnemy = true;
    }
}
