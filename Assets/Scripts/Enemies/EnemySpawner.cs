using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
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

        Instantiate(enemy, transform.position, Quaternion.identity);

        StartCoroutine(SpawnEnemy());
    }
}
