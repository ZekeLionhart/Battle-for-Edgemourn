using UnityEngine;
using UnityEngine.Pool;

public class BurningTarManager : ProjectileManager
{
    [SerializeField] private Rigidbody2D rigidBody;

    private IObjectPool<BurningTarManager> pool;
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
            pool.Release(this);
            return;
        }

        if (collision.TryGetComponent(out IDamageableEnemy receiver))
            receiver.TakeDamage(gameObject, powerType, damageType, damage);
    }

    public void SetPool(IObjectPool<BurningTarManager> pool)
    {
        this.pool = pool;
    }

    public void Launch()
    {
        inFlight = true;
        gameObject.SetActive(true);

        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
    }
}