using UnityEngine;

public class StoneWallController : CrosshairPower
{
    [SerializeField] private int hitpoints;
    [SerializeField] private int duration;
    [SerializeField] private Transform floor;

    protected override void Shoot()
    {
        WallManager wall = projectilePool.GetStonewall();

        Vector2 position = new (
            aimingPoint.position.x,
            floor.position.y + floor.localScale.y / 2f + wall.transform.localScale.y);

        wall.transform.SetPositionAndRotation(position, Quaternion.identity);
        wall.Initialize(hitpoints, duration, damage, powerType, damageType, shakeDuration, shakeIntensity);

        base.Shoot();
    }
}
