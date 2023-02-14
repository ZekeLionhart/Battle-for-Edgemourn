using System;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    [SerializeField] private float damage;

    public static Action<GameObject, float> OnEnemyHit;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
            OnEnemyHit(collision.collider.gameObject, damage);

        Destroy(this.gameObject);
    }
}
