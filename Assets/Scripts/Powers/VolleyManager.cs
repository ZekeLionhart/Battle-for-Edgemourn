using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VolleyManager : PowerManager
{
    [SerializeField] private int numOfArrows;
    private Rigidbody2D[] shotControllers;

    protected override void Awake()
    {
        base.Awake();
        shotControllers = new Rigidbody2D[numOfArrows];
        for (int i = 0; i < numOfArrows; i++)
            shotControllers[i] = arrow;
    }

    protected override void Aim(float angle)
    {
        aimingPoint.rotation = Quaternion.identity;
        aimingPoint.Rotate(0, 0, angle);
    }

    protected override void Shoot(Vector3 vector)
    {
        if (canShoot)
        {
            for (int i = 0; i < numOfArrows; i++)
                CreateArrow(vector, shotControllers[i], aimingPoint.position + Vector3.left * i * 0.3f);

            OnPowerShoot(this, cooldown);
            StartCoroutine(ShotDelay());
        }
    }
}