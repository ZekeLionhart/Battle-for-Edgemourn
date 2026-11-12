using UnityEngine;

public class BurningTarController : PowerController
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject particles;
    [SerializeField] private ParticleSystem smokeParticles;

    protected override void OnEnable()
    {
        base.OnEnable();
        PowerShooter.IsInsideArea += UpdateMouseState;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        PowerShooter.IsInsideArea -= UpdateMouseState;
    }

    private void Update()
    {
        if (TryShoot()) AttemptShooting();
    }

    private void AttemptShooting()
    {
        animator.SetTrigger(ParameterNames.Shoot);
        offCooldown = false;
        Invoke(nameof(Shoot), 0.5f);
    }

    private void UpdateMouseState(bool value)
    {
        isMouseInside = value;
    }

    protected override void Shoot()
    {
        BurningTarManager tar = projectilePool.GetTar();

        Vector2 position = aimingPoint.position + new Vector3(0.4f, 0f);
        tar.transform.SetPositionAndRotation(position, Quaternion.identity);
        tar.Initialize(powerType, damageType, damage, speed, shakeDuration, shakeIntensity);
        tar.Launch();

        Instantiate(particles, aimingPoint.position, particles.transform.rotation);

        base.Shoot();
    }

    private void ToggleSmoke()
    {
        if (smokeParticles.isPlaying)
            smokeParticles.Stop();
        else smokeParticles.Play();
    }
}
