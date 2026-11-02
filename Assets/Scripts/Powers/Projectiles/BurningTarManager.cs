using UnityEngine;

public class BurningTarManager : ProjectileManager
{
    private void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.down;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagNames.Floor))
        {
            Destroy(gameObject);
            return;
        }

        if (collision.TryGetComponent(out IDamageableEnemy receiver))
            receiver.TakeDamage(gameObject, powerType, damageType, damage);
    }
}