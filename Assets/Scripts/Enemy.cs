using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private int scoreValue;
    [SerializeField] private float attackCooldown;
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

        if (!isColliding)
            transform.position -= Time.fixedDeltaTime * speed * transform.right;
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
            health -= damageReceived;

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        OnEnemyDeath(scoreValue);
        Destroy(gameObject);
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        OnDamageDealt(target, damage);

        yield return attackCooldownWFS;

        canAttack = true;
    }
}
