using System;
using UnityEngine;

public class FireballManager : ProjectileManager
{
    [SerializeField] private GameObject smokeParticle;
    [SerializeField] private float particleFrequency;
    private WaitForSeconds particleWFS;

    public static Action<Vector3> OnTargetHit;

    private void Awake()
    {
        particleWFS = new WaitForSeconds(particleFrequency);
        SpawnSmoke();
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

    private void SpawnSmoke()
    {
        Instantiate(smokeParticle, transform.position, Quaternion.identity, null);

        Invoke(nameof(SpawnSmoke), particleFrequency);
    }
}
