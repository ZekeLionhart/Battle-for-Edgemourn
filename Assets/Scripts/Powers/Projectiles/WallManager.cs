using UnityEngine;

public class WallManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource hitSfx;
    private int hitpoints = 100;
    private float damage;
    private PowerTypes powerType;
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
        if (collision.CompareTag(TagNames.Enemy))
            ProjectileManager.OnEnemyHit(gameObject, collision.gameObject, powerType, damageType, damage);
    }

    private void SetStats(int hitpoints, float duration, float damage, PowerTypes powerType, DamageTypes damageType)
    {
        this.powerType = powerType;
        this.hitpoints = hitpoints;
        this.damage = damage;
        this.damageType = damageType;
        Invoke(nameof(ForceDeath), duration);
    }

    private void TakeDamage(GameObject target, int damage)
    {
        if (target == gameObject)
        {
            hitpoints -= damage;
            hitSfx.Play();

            if (hitpoints <= 0)
                animator.SetTrigger(ParameterNames.Destroy);
            else
                animator.SetTrigger(ParameterNames.Hit);
        }
    }

    private void ForceDeath()
    {
        TakeDamage(gameObject, 1000);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
