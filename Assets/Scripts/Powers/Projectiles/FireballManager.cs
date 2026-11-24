using UnityEngine;

public class FireballManager : ProjectileManager
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private ExplosionManager explosionPrefab;
    [SerializeField] private GameObject sparkParticle;
    [SerializeField] private float sparkFrequency;
    [SerializeField] private GameObject smokeParticle;
    [SerializeField] private float smokeFrequency;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField, Min(0.1f)] private float maxFlightTime = 5f;

    private PoolManager explosionPool;
    private bool inFlight;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!inFlight) return;

        Collider2D hit = collision.collider;
        if (!hit.CompareTag(TagNames.Floor)
            && !hit.CompareTag(TagNames.Enemy)
            && !hit.CompareTag(TagNames.Player))
            return;


        ExplosionManager explosion = explosionPool.Get(explosionPrefab);
        explosion.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        explosion.Initialize(powerType, damageType, damage, speed, shakeDuration, shakeIntensity);
        explosion.Play();

        ReturnToPool();
    }

    public void Launch(Vector3 velocity, PoolManager projectilePool)
    {
        this.explosionPool = projectilePool;
        inFlight = true;

        gameObject.SetActive(true);
        rigidBody.linearVelocity = velocity;
        rigidBody.angularVelocity = 0f;

        SpawnSparks();
        SpawnSmoke();
        SpawnMuzzle();
        Invoke(nameof(ReturnToPool), maxFlightTime);
    }

    private void ReturnToPool()
    {
        inFlight = false;
        CancelInvoke();
        ReleaseToPool();
    }

    private void SpawnSparks()
    {
        SpawnEffect(sparkParticle, Quaternion.identity);
        Invoke(nameof(SpawnSparks), sparkFrequency);
    }

    private void SpawnSmoke()
    {
        SpawnEffect(smokeParticle, Quaternion.identity);
        Invoke(nameof(SpawnSmoke), smokeFrequency);
    }

    private void SpawnMuzzle()
    {
        SpawnEffect(muzzleFlash, Quaternion.identity);
    }

    private void SpawnEffect(GameObject template, Quaternion rotation)
    {
        GameObject effect = Instantiate(template, template.transform.position, rotation);
        effect.SetActive(true);
    }
}
