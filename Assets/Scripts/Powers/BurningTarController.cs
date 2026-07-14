using UnityEngine;

public class BurningTarController : PowerController
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject particles;
    [SerializeField] private ParticleSystem smokeParticles;  

    protected override void AttemptShooting()
    {
        if (canShoot && isActive)
        {
            animator.SetTrigger(ParameterNames.Shoot);
            canShoot = false;
            Invoke(nameof(Shoot), 0.5f);
        }
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
