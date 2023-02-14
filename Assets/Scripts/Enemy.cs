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
    private bool isColliding;

    public static Action<int> OnDamageDealt;
    public static Action<int> OnEnemyDeath;

    private void OnEnable()
    {
        ShotController.OnEnemyHit += TakeDamage;
    }

    private void OnDisable()
    {
        ShotController.OnEnemyHit -= TakeDamage;
    }

    void FixedUpdate()
    {
        if (!isColliding)
            transform.position -= Time.fixedDeltaTime * speed * transform.right;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Tower"))
        {
            isColliding = true;
            OnDamageDealt(damage);
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
}
