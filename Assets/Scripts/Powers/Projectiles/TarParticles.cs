using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem tarParticles;
    [SerializeField] private ParticleSystem smokeParticles;
    [SerializeField] private GameObject hissing;
    [SerializeField] private float timer;

    private void Start()
    {
        StartCoroutine(SelfDestroy());
    }

    private void OnParticleCollision(GameObject other)
    {
        List<ParticleCollisionEvent> collisionEvent = new();
        tarParticles.GetCollisionEvents(other, collisionEvent);

        hissing.SetActive(true);
        Instantiate(smokeParticles, collisionEvent[0].intersection - new Vector3(0f,0.25f), smokeParticles.transform.rotation, transform);

    }

    private IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(timer);

        Destroy(gameObject);
    }
}
