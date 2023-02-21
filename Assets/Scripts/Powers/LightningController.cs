using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LightningController : CrosshairPower
{
    protected override void Shoot()
    {
        Rigidbody2D shotRigid = Instantiate(shot, new Vector2(aimingPoint.transform.position.x, 6f)
            , shot.transform.rotation);

        OnShotInstantiated(shotRigid, damage);
        base.Shoot();
    }
}
