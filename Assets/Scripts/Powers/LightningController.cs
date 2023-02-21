using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LightningController : CrosshairPower
{
    public static Action<Rigidbody2D, float> OnLightningInstantiated;

    protected override void Shoot()
    {
        Rigidbody2D shotRigid = Instantiate(shot, new Vector2(aimingPoint.transform.position.x, 6f)
            , shot.transform.rotation);

        OnLightningInstantiated(shotRigid, damage);
        base.Shoot();
    }
}
