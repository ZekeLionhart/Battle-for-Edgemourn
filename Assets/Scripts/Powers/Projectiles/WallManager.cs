using UnityEngine;

public class WallManager : MonoBehaviour
{
    private int hitpoints = 100;
    private float damage;
    private DamageTypes damageType;

    private void OnEnable()
    {
        StoneWallController.OnWallSummon += SetStats;
        EnemyBase.OnDamageDealt += TakeDamage;
    }

    private void OnDisable()
    {
        StoneWallController.OnWallSummon -= SetStats;
        EnemyBase.OnDamageDealt -= TakeDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            ProjectileManager.OnEnemyHit(collision.gameObject, damageType, damage);
    }

    private void SetStats(int hitpoints, float damage, DamageTypes damageType)
    {
        this.hitpoints = hitpoints;
        this.damage = damage;
        this.damageType = damageType;
    }

    private void TakeDamage(GameObject target, int damage)
    {
        if (target == gameObject)
        {
            hitpoints -= damage;

            if (hitpoints <= 0)
                Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
