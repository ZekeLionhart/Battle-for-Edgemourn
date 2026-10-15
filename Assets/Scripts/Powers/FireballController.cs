using UnityEngine;

public class FireballController : CrosshairPower
{
    [SerializeField] private FireballManager fireballPrefab;
    [SerializeField] private float speed;
    [SerializeField] private Transform direction;
    [SerializeField] private ExplosionManager explosion;

    protected override void Update()
    {
        HandleAimRotation();

        if (TryShoot()) Shoot();
    }

    protected override void Shoot()
    {
        FireballManager fireball = Instantiate(fireballPrefab, transform.position, aimingPoint.transform.rotation);

        fireball.Initialize(powerType, damageType, damage, speed, shakeDuration, shakeIntensity);

        Vector3 velocity = speed * (direction.position - aimingPoint.position);
        fireball.Launch(velocity, explosion);

        base.Shoot();
    }
}
