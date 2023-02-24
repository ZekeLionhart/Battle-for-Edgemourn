using System;
using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private float hitpoints;
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int scoreValue;
    private GameObject target;
    private WaitForSeconds attackCooldownWFS;
    private bool isColliding = false;
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

    private void FixedUpdate()
    {
        if (canAttack && isColliding)
            StartCoroutine(Attack());

        //WalkForwards();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Tower") || collision.collider.CompareTag("Player"))
        {
            isColliding = true;
            canAttack = true;
            target = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Tower") || collision.collider.CompareTag("Player"))
        {
            isColliding = false;
            canAttack = false;
            target = null;
        }
    }

    private void TakeDamage(GameObject target, float damageReceived)
    {
        if (target == gameObject)
            hitpoints -= damageReceived;
    }

    private void WalkForwards()
    {
        transform.position -= Time.fixedDeltaTime * speed * transform.right;
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        OnDamageDealt(target, damage);

        yield return attackCooldownWFS;

        canAttack = true;
    }

    private void Die()
    {
        OnEnemyDeath(scoreValue);
        Destroy(gameObject);
    }
}
