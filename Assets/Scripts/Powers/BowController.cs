using UnityEngine;

public class BowController : ArrowPower
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
            CreateArrow(vector, shot, aimingPoint.position);

            OnPowerShoot(this,cooldown);
            canShoot = false;
        }
    }
}