using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ShotController shot;
    [SerializeField] private Transform aimingPoint;
    [SerializeField] private GameObject enemy;
    private WaitForSeconds delay;
    private bool canSpawnEnemy = true;

    private void Awake()
    {
        delay = new WaitForSeconds(2f);
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
        aimingPoint.rotation = Quaternion.identity;
        aimingPoint.Rotate(0, 0, angle);

        speed *= 2;
        if (speed > 15)
            speed = 15;
        shot.speed = speed;

        Instantiate(shot, aimingPoint.position, aimingPoint.rotation);
    }

    private IEnumerator SpawnEnemy()
    {
        canSpawnEnemy = false;

        yield return delay;

        Instantiate(enemy, new Vector3(10f, -2f, -1f), Quaternion.identity);

        canSpawnEnemy = true;
    }
}
