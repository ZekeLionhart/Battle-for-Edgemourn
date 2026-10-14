using UnityEngine;

public class StoneWallController : CrosshairPower
{
    [SerializeField] private WallManager wallPrefab;
    [SerializeField] private int hitpoints;
    [SerializeField] private int duration;
    [SerializeField] private Transform floor;

    protected override void Shoot()
    {
        Vector2 position = new (
            aimingPoint.transform.position.x,
            floor.position.y + floor.localScale.y / 2 + shot.transform.localScale.y);

        WallManager wall = Instantiate(wallPrefab, position, wallPrefab.transform.rotation);

        wall.Initialize(hitpoints, duration, damage, powerType, damageType, shakeDuration, shakeIntensity);

        base.Shoot();
    }
}
