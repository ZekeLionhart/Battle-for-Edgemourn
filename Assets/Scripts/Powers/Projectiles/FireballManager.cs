using UnityEngine;

public class FireballManager : ProjectileManager
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
            OnEnemyHit(collision.collider.gameObject, damageType, damage);

        Destroy(gameObject);
    }
}
