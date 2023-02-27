using System;
using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float hitpoints;
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int scoreValue;
    private GameObject target;
    private WaitForSeconds attackCooldownWFS;

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

    protected virtual void TakeDamage(GameObject target, DamageTypes damageType, float damageReceived)
    {
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

    private void Attack()
    {
        OnDamageDealt(target, damage);

        StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        yield return attackCooldownWFS;

        animator.SetTrigger("OnAttackCldwn");
    }

    private void Die()
    {
        OnEnemyDeath(scoreValue);
        Destroy(gameObject);
    }
}
