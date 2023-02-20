using System;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    private float damage;
    private bool canDamage = true;

    public static Action<GameObject, float> OnEnemyHit;

    private void OnEnable()
    {
        PowerManager.OnShotInstantiated += SetDamage;
    }

    private void OnDisable()
    {
        PowerManager.OnShotInstantiated -= SetDamage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") && canDamage)
        {
            OnEnemyHit(collision.collider.gameObject, damage);
            canDamage = false;
        }

        Destroy(this.gameObject);
    }

    private void SetDamage(Rigidbody2D shot, float damage)
    {
        if (shot.gameObject == this.gameObject)
            this.damage = damage;
    }
}
