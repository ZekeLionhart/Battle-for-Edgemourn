using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ShotController shot;
    [SerializeField] private Transform aimingPoint;
    [SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private GameObject enemy;
    [SerializeField] private float enemyDelay;
    [SerializeField] private float shotDelay;
    private WaitForSeconds enemyDelayWFS;
    private WaitForSeconds shotDelayWFS;
    private bool canSpawnEnemy = true;
    private bool canShoot = true;

    private void Awake()
    {
        enemyDelayWFS = new WaitForSeconds(enemyDelay);
        shotDelayWFS = new WaitForSeconds(shotDelay);
    }

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
        if (canShoot)
        {
            aimingPoint.rotation = Quaternion.identity;
            aimingPoint.Rotate(0, 0, angle); 
            
            shot.speed = speed;
            Instantiate(shot, aimingPoint.position, aimingPoint.rotation);

            StartCoroutine(ShootDelay());
        }
    }

    private IEnumerator ShootDelay()
    {
        canShoot = false;

        yield return shotDelayWFS;

        canShoot = true;
    }

    private IEnumerator SpawnEnemy()
    {
        canSpawnEnemy = false;

        yield return enemyDelayWFS;

        Instantiate(enemy, enemySpawnPoint.position, Quaternion.identity);

        canSpawnEnemy = true;
    }
}
