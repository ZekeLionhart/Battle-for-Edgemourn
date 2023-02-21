using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VolleyController : ArrowPower
{
    [SerializeField] private int numOfArrows;
    [SerializeField] private float spreadMultiplier;
    private Rigidbody2D[] arrows;

    protected override void Awake()
    {
        base.Awake();
        arrows = new Rigidbody2D[numOfArrows];
        for (int i = 0; i < numOfArrows; i++)
            arrows[i] = shot;
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
                CreateArrow(vector, arrows[i], aimingPoint.position + Vector3.left * i * spreadMultiplier);

            OnPowerShoot(this, cooldown);
            canShoot = false;
        }
    }
}