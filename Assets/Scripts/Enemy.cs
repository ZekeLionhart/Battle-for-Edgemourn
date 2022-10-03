using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;

    private void OnEnable()
    {
        ShotController.OnEnemyHit += DestroySelf;
    }

    private void OnDisable()
    {
        ShotController.OnEnemyHit -= DestroySelf;
    }

    void FixedUpdate()
    {
        transform.position -= transform.right * speed * Time.fixedDeltaTime;
    }

    private void DestroySelf(GameObject target)
    {
        if (target == this.gameObject)
            Destroy(this.gameObject);
    }
}
