using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] protected GameObject enemy;
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

        Instantiate(enemy, transform.position, Quaternion.identity);

        StartCoroutine(SpawnEnemy());
    }
}
