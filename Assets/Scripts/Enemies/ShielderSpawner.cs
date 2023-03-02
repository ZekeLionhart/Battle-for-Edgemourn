using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShielderSpawner : EnemySpawner
{
    [SerializeField] private int squadSize;
    [SerializeField] private float distBetweenEach;

    protected override IEnumerator SpawnEnemy()
    {
        yield return enemyWFS;

        for (int i = 0; i < squadSize; i++)
            Instantiate(enemy, transform.position + distBetweenEach * i * Vector3.right, Quaternion.identity);

        StartCoroutine(SpawnEnemy());
    }
}
