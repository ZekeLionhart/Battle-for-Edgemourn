using UnityEngine;

public class BurningTarManager : ProjectileManager
{
    [SerializeField] private Rigidbody2D rigidBody;

    private bool inFlight;

    private void Update()
    {
        transform.position += speed * Time.deltaTime * Vector3.down;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!inFlight) return;

        if (collision.CompareTag(TagNames.Floor))
        {
            inFlight = false;
            ReleaseToPool();
            return;
        }

        if (collision.TryGetComponent(out IDamageableEnemy receiver))
            receiver.TakeDamage(this, powerType, damageType, damage);
    }

    public void Launch()
    {
        inFlight = true;
        gameObject.SetActive(true);

        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
    }
}