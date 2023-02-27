using UnityEngine;

public class BowController : PowerController
{
    [SerializeField] protected float arrowSpeed;

    protected override void OnEnable()
    {
        base.OnEnable();
        LineDrawer.OnMouseUp += Shoot;
        LineDrawer.OnAimUpdate += Aim;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        LineDrawer.OnMouseUp -= Shoot;
        LineDrawer.OnAimUpdate -= Aim;
    }

    private void Aim(float angle)
    {
        aimingPoint.rotation = Quaternion.identity;
        aimingPoint.Rotate(0, 0, angle);
    }

    protected virtual void Shoot(Vector3 vector)
    {
        if (canShoot)
            CreateArrow(vector, shot, aimingPoint.position);
    }

    protected void CreateArrow(Vector3 vector, Rigidbody2D arrow, Vector3 position)
    {
        Rigidbody2D shotRigid = Instantiate(arrow, position, aimingPoint.rotation);
        Vector3 force = -1f * arrowSpeed * vector;
        shotRigid.velocity = force;

        OnShotInstantiated(shotRigid, damageType, damage, 0f);
        OnPowerShoot(this, cooldown);
        canShoot = false;
    }
}
