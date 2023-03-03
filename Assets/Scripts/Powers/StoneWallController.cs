using System;
using UnityEngine;

public class StoneWallController : CrosshairPower
{
    [SerializeField] private int hitpoints;
    [SerializeField] private Transform floor;

    public static Action<int, float, DamageTypes> OnWallSummon;

    protected override void Shoot()
    {
        Instantiate(shot, new Vector2(
            aimingPoint.transform.position.x, floor.position.y + floor.localScale.y / 2 - shot.transform.localScale.y / 2)
            , shot.transform.rotation);

        OnWallSummon(hitpoints, damage, damageType);
        base.Shoot();
    }
}
