using UnityEngine;

public class LightningController : CrosshairPower
{
    protected override void Shoot()
    {
        GameObject newShot = Instantiate(shot.gameObject, new Vector2(aimingPoint.transform.position.x, 1.5f)
            , Quaternion.identity);

        OnShotInstantiated(newShot, damageType, damage, 0f);
        base.Shoot();
    }
}
