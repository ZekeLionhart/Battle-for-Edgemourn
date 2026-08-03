using System.Collections;
using UnityEngine;

public class UnlimitedSpawner : MonoBehaviour
{
    [SerializeField] private EnemyBase enemy;
    [SerializeField] private int squadSize;
    [SerializeField] private float distBetweenEach;
    [SerializeField] private float enemyDelay;
    private WaitForSeconds enemyWFS;

    private void Awake()
    {
        enemyWFS = new WaitForSeconds(enemyDelay);
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        yield return enemyWFS;

        for (int i = 0; i < squadSize; i++)
            Instantiate(enemy, transform.position + distBetweenEach * i * Vector3.right, transform.rotation);

        StartCoroutine(SpawnEnemy());
    }
}
