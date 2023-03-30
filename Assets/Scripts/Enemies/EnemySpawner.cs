using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemySpawnSettings> enemyList;
    private float lastSpawn;

    private void FixedUpdate()
    {
        if (enemyList.Count == 0)
            return;

        if (Time.time > lastSpawn + enemyList[0].secondsAfterLastEnemy)
        {
            Instantiate(enemyList[0].enemy.enemyPrefab, transform.position, transform.rotation);
            RegisterLastSpawnTime();
            DequeueEnemy();
        }
    }

    private void RegisterLastSpawnTime()
    {
        lastSpawn = Time.time;
    }

    private void DequeueEnemy()
    {
        enemyList.RemoveAt(0);
    }
}
