using UnityEngine;

public class LightningController : CrosshairPower
{
    [SerializeField] private ExplosionManager lightningPrefab;

    protected override void Shoot()
    {
        Vector2 position = new(aimingPoint.position.x, 1.35f);

        ExplosionManager lightning = Instantiate(lightningPrefab, position, Quaternion.identity);

        lightning.Initialize(powerType, damageType, damage, 0f, shakeDuration, shakeIntensity);

        base.Shoot();
    }
}
