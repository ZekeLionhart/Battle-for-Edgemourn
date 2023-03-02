using UnityEngine;

public class LightningController : CrosshairPower
{
    protected override void Shoot()
    {
        Rigidbody2D shotRigid = Instantiate(shot, new Vector2(aimingPoint.transform.position.x, 6f)
            , shot.transform.rotation);

        OnShotInstantiated(shotRigid.gameObject, damageType, damage, 0f);
        base.Shoot();
    }
}
