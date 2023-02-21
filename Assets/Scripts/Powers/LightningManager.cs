using System;
using UnityEngine;

public class LightningManager : MonoBehaviour
{
    private float damage;

    public static Action<GameObject, float> OnEnemyHit;

    private void OnEnable()
    {
        LightningController.OnLightningInstantiated += SetDamage;
    }

    private void OnDisable()
    {
        LightningController.OnLightningInstantiated -= SetDamage;
    }

    private void Update()
    {
        transform.position += 20f * Time.deltaTime * Vector3.down;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
            OnEnemyHit(collision.collider.gameObject, damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor"))
            Destroy(gameObject);
    }

    private void SetDamage(Rigidbody2D shot, float damage)
    {
        if (shot.gameObject == gameObject)
            this.damage = damage;
    }
}
