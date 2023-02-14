using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public static Action<GameObject> OnEnemyHit;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
            OnEnemyHit(collision.collider.gameObject);

        Destroy(this.gameObject);
    }
}
