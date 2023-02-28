using UnityEngine;

public class ArrowManager : ProjectileManager
{
    private bool canDamage = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") && canDamage)
        {
            OnEnemyHit(collision.collider.gameObject, damageType, damage);
            canDamage = false;
        }

        Destroy(gameObject);
    }
}
