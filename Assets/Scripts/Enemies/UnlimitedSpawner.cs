using System.Collections;
using UnityEngine;

public class UnlimitedSpawner : MonoBehaviour
{
    [SerializeField] private EnemyConfig enemy;
    [SerializeField] private int squadSize;
    [SerializeField] private float distBetweenEach;
    [SerializeField] private float enemyDelay;
    protected WaitForSeconds enemyWFS;

    private void Awake()
    {
        enemyWFS = new WaitForSeconds(enemyDelay);
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    protected virtual IEnumerator SpawnEnemy()
    {
        yield return enemyWFS;

        for (int i = 0; i < squadSize; i++)
            Instantiate(enemy.enemyPrefab, transform.position + distBetweenEach * i * Vector3.right, transform.rotation);

        StartCoroutine(SpawnEnemy());
    }
}
