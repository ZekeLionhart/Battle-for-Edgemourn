using System;
using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    [SerializeField] private float hitpoints;
    [SerializeField] private float speed;
    [SerializeField] protected int damage;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int scoreValue;
    [SerializeField] protected float arrowMultiplier;
    [SerializeField] protected float fireMultiplier;
    [SerializeField] protected float thunderMultiplier;
    [SerializeField] protected float earthMultiplier;
    private GameObject target;
    private WaitForSeconds attackCooldownWFS;
    private bool canAttack = true;

    public static Action<GameObject, int> OnDamageDealt;
    public static Action<int> OnEnemyDeath;

    private void Awake()
    {
        attackCooldownWFS = new WaitForSeconds(attackCooldown);
    }

    private void OnEnable()
    {
        ProjectileManager.OnEnemyHit += TakeDamage;
    }

    private void OnDisable()
    {
        ProjectileManager.OnEnemyHit -= TakeDamage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Tower") || collision.collider.CompareTag("Player"))
        {
            animator.SetBool("IsColliding", true);
            target = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Tower") || collision.collider.CompareTag("Player"))
        {
            animator.SetBool("IsColliding", false);
            target = null;
        }
    }

    protected virtual float MultiplyDamage(DamageTypes damageType, float damageReceived)
    {
        switch (damageType)
        {
            case DamageTypes.Pierce:
                damageReceived *= arrowMultiplier;
                break;

            case DamageTypes.Fire:
                damageReceived *= fireMultiplier;
                break;

            case DamageTypes.Thunder:
                damageReceived *= thunderMultiplier;
                break;

            case DamageTypes.Earth:
                damageReceived *= earthMultiplier;
                break;

            default:
                break;
        }

        return damageReceived;
    }

    protected virtual void TakeDamage(GameObject target, DamageTypes damageType, float damageReceived)
    {
        damageReceived = MultiplyDamage(damageType, damageReceived);

        if (target == gameObject)
            hitpoints -= damageReceived;

        if (hitpoints <= 0)
            animator.SetTrigger("OnHpEmpty");
    }

    private void WalkForwards()
    {
        animator.ResetTrigger("OnAttackCldwn");

        transform.position -= Time.fixedDeltaTime * speed * transform.right;
    }

    protected virtual void Attack()
    {
        OnDamageDealt(target, damage);
    }

    protected IEnumerator AttackCooldown()
    {
        if (canAttack)
        {
            canAttack = false;
            yield return attackCooldownWFS;

            animator.SetTrigger("OnAttackCldwn");
            canAttack = true;
        }
    }

    private void Die()
    {
        OnEnemyDeath(scoreValue);
        Destroy(gameObject);
    }
}