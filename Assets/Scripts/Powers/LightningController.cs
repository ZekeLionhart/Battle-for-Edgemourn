using UnityEngine;

public class LightningController : CrosshairPower
{
    protected override void Shoot()
    {
        Vector2 position = new(aimingPoint.position.x, 1.35f);

        ExplosionManager lightning = projectilePool.GetLightning();
        lightning.transform.SetPositionAndRotation(position, Quaternion.identity);
        lightning.Initialize(powerType, damageType, damage, 0f, shakeDuration, shakeIntensity);
        lightning.Play();

        base.Shoot();
    }
}
