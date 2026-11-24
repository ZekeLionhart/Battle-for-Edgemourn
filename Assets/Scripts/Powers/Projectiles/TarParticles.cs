using System.Collections.Generic;
using UnityEngine;

public class TarParticles : PoolableObject
{
    [SerializeField] private ParticleSystem tarParticles;
    [SerializeField] private TarSmokeVFX smokeParticles;
    [SerializeField] private GameObject hissing;
    [SerializeField] private float timer;

    private readonly List<ParticleCollisionEvent> collisionEvents = new();
    private readonly List<TarSmokeVFX> activeSmoke = new();

    private PoolManager vfxPool;
    private AudioSource[] sounds;

    private void Awake()
    {
        sounds = GetComponentsInChildren<AudioSource>(true);
    }

    public void Initialize(PoolManager vfxPool)
    {
        this.vfxPool = vfxPool;

        hissing.SetActive(false);
        tarParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        gameObject.SetActive(true);
        tarParticles.Play(true);

        Invoke(nameof(ReturnToPool), timer);
    }   

    private void OnParticleCollision(GameObject other)
    {
        int collisionCount = tarParticles.GetCollisionEvents(other, collisionEvents);
        if (collisionCount == 0) return;

        hissing.SetActive(true);

        TarSmokeVFX smoke = vfxPool.Get(smokeParticles);
        smoke.transform.SetParent(transform, false);
        smoke.transform.SetPositionAndRotation(
            collisionEvents[0].intersection - new Vector3(0f, 0.25f, 0f),
            smokeParticles.transform.rotation);

        activeSmoke.Add(smoke);
        smoke.Initialize();
    }

    private void ReturnToPool()
    {
        CancelInvoke(nameof(ReturnToPool));

        foreach (TarSmokeVFX smoke in activeSmoke)
            smoke.ReturnToPool();

        activeSmoke.Clear();
        collisionEvents.Clear();

        tarParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        foreach (AudioSource sound in sounds)
            sound.Stop();

        ReleaseToPool();
    }
}
