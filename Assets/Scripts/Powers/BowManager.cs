using System;
using System.Collections;
using UnityEngine;

public class BowManager : PowerManager
{
    protected override void Aim(float angle)
    {
        aimingPoint.rotation = Quaternion.identity;
        aimingPoint.Rotate(0, 0, angle);
    }

    protected override void Shoot(Vector3 vector)
    {
        if (canShoot)
        {
            CreateArrow(vector, arrow, aimingPoint.position);

            OnPowerShoot(this,cooldown);
            StartCoroutine(ShotDelay());
        }
    }
}