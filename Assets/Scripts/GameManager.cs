using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ShotController shot;
    [SerializeField] private Transform spawn;
    [SerializeField] private GameObject enemy;
    private bool canSpawnEnemy = true;

    private void OnEnable()
    {
        LineDrawer.OnMouseUp += Shoot;
    }

    private void OnDisable()
    {
        LineDrawer.OnMouseUp -= Shoot;
    }

    private void Update()
    {
        if (canSpawnEnemy)
            StartCoroutine(SpawnEnemy());
    }

    private void Shoot(float speed, float angle)
    {
        spawn.rotation = Quaternion.identity;
        spawn.Rotate(0, 0, angle);
        speed *= 2;
        if (speed > 15)
            speed = 15;
        shot.speed = speed;
        Instantiate(shot, spawn.position, spawn.rotation);
    }

    private IEnumerator SpawnEnemy()
    {
        canSpawnEnemy = false;

        yield return new WaitForSeconds(2f);

        Instantiate(enemy, new Vector3(10f, -2f, -1f), Quaternion.identity);

        canSpawnEnemy = true;
    }
}
