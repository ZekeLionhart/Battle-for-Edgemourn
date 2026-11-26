using UnityEngine;

public class FireballManager : ProjectileManager
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private ExplosionManager explosionPrefab;
    [SerializeField] private SpriteRenderer[] bodySprites;
    [SerializeField] private AudioSource flightSFX;
    [SerializeField] private ParticleSystem sparks;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private ParticleSystem muzzleSmoke;
    [SerializeField, Min(0.1f)] private float maxFlightTime = 5f;

    private PoolManager explosionPool;
    private bool inFlight;

    private void Update()
    {
        if (inFlight) return;

        if (!sparks.IsAlive(false) && !smoke.IsAlive(false) && !muzzleSmoke.IsAlive(false))
            ReleaseToPool();
    }

    private void OnDisable()
    {
        CancelInvoke();
        inFlight = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!inFlight) return;

        Collider2D hit = collision.collider;
        if (!hit.CompareTag(TagNames.Floor)
            && !hit.CompareTag(TagNames.Enemy)
            && !hit.CompareTag(TagNames.Player))
            return;

        StopFlight();

        ExplosionManager explosion = explosionPool.Get(explosionPrefab);
        explosion.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        explosion.Initialize(powerType, damageType, damage, speed, shakeDuration, shakeIntensity);
        explosion.Play();
    }

    public void Launch(Vector3 velocity, PoolManager projectilePool)
    {
        this.explosionPool = projectilePool;
        inFlight = true;

        gameObject.SetActive(true);
        rigidBody.simulated = true;
        rigidBody.linearVelocity = velocity;
        rigidBody.angularVelocity = 0f;

        foreach (SpriteRenderer sprite in bodySprites)
            sprite.enabled = true;

        PlayParticles(sparks);
        PlayParticles(smoke);
        PlayParticles(muzzleSmoke);
        Invoke(nameof(StopFlight), maxFlightTime);
    }

    private void StopFlight()
    {
        if (!inFlight) return;

        inFlight = false;
        CancelInvoke();
        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.angularVelocity = 0f;
        rigidBody.simulated = false;

        foreach (SpriteRenderer sprite in bodySprites)
            sprite.enabled = false;

        flightSFX.Stop();
        sparks.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        smoke.Stop(false, ParticleSystemStopBehavior.StopEmitting);
    }

    private void PlayParticles(ParticleSystem particles)
    {
        particles.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
        particles.Play(false);
    }
}
