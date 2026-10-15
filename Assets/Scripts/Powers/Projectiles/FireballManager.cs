using UnityEngine;

public class FireballManager : ProjectileManager
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private GameObject sparkParticle;
    [SerializeField] private float sparkFrequency;
    [SerializeField] private GameObject smokeParticle;
    [SerializeField] private float smokeFrequency;
    [SerializeField] private GameObject muzzleFlash;
    private ExplosionManager explosionPrefab;
    private bool hasExploded;

    private void Awake()
    {
        SpawnSparks();
        SpawnSmoke();
        SpawnMuzzle();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasExploded) return;

        if (collision.collider.CompareTag(TagNames.Floor) 
            || collision.collider.CompareTag(TagNames.Enemy) 
            || collision.collider.CompareTag(TagNames.Player)) 
        {
            hasExploded = true;

            ExplosionManager explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            explosion.Initialize(powerType, damageType, damage, speed, shakeDuration, shakeIntensity);

            Destroy(gameObject);
        }
    }

    public void Launch(Vector3 velocity, ExplosionManager explosionPrefab)
    {
        this.explosionPrefab = explosionPrefab;
        rigidBody.velocity = velocity;
    }

    private void SpawnSparks()
    {
        Instantiate(sparkParticle, sparkParticle.transform.position, Quaternion.identity, null);
        
        Invoke(nameof(SpawnSparks), sparkFrequency);
    }

    private void SpawnSmoke()
    {
        Instantiate(smokeParticle, smokeParticle.transform.position, Quaternion.identity, null);

        Invoke(nameof(SpawnSmoke), smokeFrequency);
    }

    private void SpawnMuzzle()
    {
        Instantiate(muzzleFlash, muzzleFlash.transform.position, muzzleFlash.transform.rotation, null);
    }
}
