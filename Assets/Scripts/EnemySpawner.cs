using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private float[] enemyDelays;

    private void Start()
    {
        foreach (GameObject enemy in enemies)
            StartCoroutine(SpawnEnemy(System.Array.IndexOf(enemies, enemy)));
    }

    private IEnumerator SpawnEnemy(int index)
    {
        yield return new WaitForSeconds(enemyDelays[index]);

        Instantiate(enemies[index], transform.position, Quaternion.identity);

        StartCoroutine(SpawnEnemy(index));
    }
}
