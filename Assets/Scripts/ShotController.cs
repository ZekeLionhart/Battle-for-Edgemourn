using System;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    [SerializeField] private float damage;
    private bool canDamage = true;

    public static Action<GameObject, float> OnEnemyHit;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") && canDamage)
        {
            OnEnemyHit(collision.collider.gameObject, damage);
            canDamage = false;
        }

        Destroy(this.gameObject);
    }
}
