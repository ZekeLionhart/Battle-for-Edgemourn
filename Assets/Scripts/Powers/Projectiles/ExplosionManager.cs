using UnityEngine;

public class ExplosionManager : ProjectileManager
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagNames.Enemy))
            OnEnemyHit(collision.gameObject, damageType, damage);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}