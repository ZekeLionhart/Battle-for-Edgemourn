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
        base.OnEnable();
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
        Rigidbody2D shotRigid = Instantiate(shot, aimingPoint.transform.position + new Vector3(0.4f, 0f), shot.transform.rotation);

        Instantiate(particles, aimingPoint.transform.position, particles.transform.rotation);
        
        OnShotInstantiated(shotRigid.gameObject, powerType, damageType, damage, speed, shakeDuration, shakeIntensity);
        base.Shoot();
    }

    private void ToggleSmoke()
    {
        if (smokeParticles.isPlaying)
            smokeParticles.Stop();
        else smokeParticles.Play();
    }
}
