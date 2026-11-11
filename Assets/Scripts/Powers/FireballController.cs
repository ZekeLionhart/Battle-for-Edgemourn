using UnityEngine;

public class FireballController : CrosshairPower
{
    [SerializeField] private float speed;
    [SerializeField] private Transform direction;

    protected override void Update()
    {
        HandleAimRotation();

        if (TryShoot()) Shoot();
    }

    protected override void Shoot()
    {
        FireballManager fireball = projectilePool.GetFireball();
        fireball.transform.SetPositionAndRotation(transform.position, aimingPoint.rotation);
        fireball.Initialize(powerType, damageType, damage, speed, shakeDuration, shakeIntensity);

        Vector3 velocity = speed * (direction.position - aimingPoint.position);
        fireball.Launch(velocity, projectilePool);
    
        base.Shoot();
    }
}
