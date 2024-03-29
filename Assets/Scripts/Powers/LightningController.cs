using UnityEngine;

public class LightningController : CrosshairPower
{
    protected override void Shoot()
    {
        if (isActive)
        {
            GameObject newShot = Instantiate(shot.gameObject, new Vector2(aimingPoint.transform.position.x, 1.35f)
                , Quaternion.identity);

            OnShotInstantiated(newShot, powerType, damageType, damage, 0f);
            base.Shoot();
        }
    }
}
