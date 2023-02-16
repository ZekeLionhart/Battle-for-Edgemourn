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
    private WaitForSeconds attackCooldownWFS;
    private bool isColliding = false;
    private bool canAttack = true;

    public static Action<int> OnDamageDealt;
    public static Action<int> OnEnemyDeath;

    private void Awake()
    {
        attackCooldownWFS = new WaitForSeconds(attackCooldown);
    }

    private void OnEnable()
    {
        ShotController.OnEnemyHit += TakeDamage;
    }

    private void OnDisable()
    {
        ShotController.OnEnemyHit -= TakeDamage;
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
        if (collision.collider.CompareTag("Tower"))
        {
            isColliding = true;
            canAttack = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Tower"))
        {
            isColliding = false;
            canAttack = false;
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
        Destroy(this.gameObject);
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        OnDamageDealt(damage);

        yield return attackCooldownWFS;

        canAttack = true;
    }
}
