using System;
using UnityEngine;

public class FireballManager : ProjectileManager
{
    [SerializeField] private GameObject sparkParticle;
    [SerializeField] private float sparkFrequency;
    [SerializeField] private GameObject smokeParticle;
    [SerializeField] private float smokeFrequency;
    [SerializeField] private GameObject muzzleFlash;

    public static Action<Vector3> OnTargetHit;

    private void Awake()
    {
        SpawnSparks();
        SpawnSmoke();
        SpawnMuzzle();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(TagNames.Floor) 
            || collision.collider.CompareTag(TagNames.Enemy) 
            || collision.collider.CompareTag(TagNames.Player)) 
        {
            OnTargetHit(transform.position);
            Destroy(gameObject);
        }
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
