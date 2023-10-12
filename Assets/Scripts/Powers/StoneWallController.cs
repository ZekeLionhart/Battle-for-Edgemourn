using System;
using UnityEngine;

public class StoneWallController : CrosshairPower
{
    [SerializeField] private int hitpoints;
    [SerializeField] private int duration;
    [SerializeField] private Transform floor;

    public static Action<int, float, float, PowerTypes, DamageTypes> OnWallSummon;

    protected override void Shoot()
    {
        if (isActive)
        {
            Instantiate(shot, new Vector2(
                aimingPoint.transform.position.x, floor.position.y + floor.localScale.y / 2 + shot.transform.localScale.y)
                , shot.transform.rotation);

            OnWallSummon(hitpoints, duration, damage, powerType, damageType);
            base.Shoot();
        }
    }
}
