using UnityEngine;

public class GoreVFX : PoolableObject
{
    [SerializeField] private ParticleSystem particles;

    public void Initialize()
    {
        particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        gameObject.SetActive(true);
        particles.Play(true);
    }

    private void Update()
    {
        if (particles.IsAlive(true)) return;

        particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ReleaseToPool();
    }
}
