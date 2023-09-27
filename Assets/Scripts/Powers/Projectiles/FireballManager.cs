using System;
using System.Collections;
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
        //StartCoroutine(SpawnSmoke());
        SpawnSmoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor") 
            || collision.collider.CompareTag("Enemy") 
            || collision.collider.CompareTag("Player")) 
        {
            OnTargetHit(transform.position);
            Destroy(gameObject);
        }
    }

    /*private IEnumerator SpawnSmoke()
    {
        Instantiate(smokeParticle, transform);

        yield return particleWFS;

        StartCoroutine(SpawnSmoke());
    }*/

    private void SpawnSmoke()
    {
        Instantiate(smokeParticle, transform.position, Quaternion.identity, null);

        Invoke(nameof(SpawnSmoke), particleFrequency);
    }
}
