using UnityEngine;

public class TarSmokeVFX : PoolableObject
{
    [SerializeField] private ParticleSystem particles;

    public void Initialize()
    {
        particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        gameObject.SetActive(true);
        particles.Play(true);
    }

    public void ReturnToPool()
    {
        particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ReleaseToPool();
    }
}
