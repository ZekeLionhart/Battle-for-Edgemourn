using UnityEngine;

public class BurningTarManager : ProjectileManager
{
    private void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.down;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(TagNames.Enemy))
            OnEnemyHit(collision.collider.gameObject, damageType, damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagNames.Floor))
            Destroy(gameObject);
    }
}