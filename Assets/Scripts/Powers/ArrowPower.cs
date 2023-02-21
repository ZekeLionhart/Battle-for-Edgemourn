using UnityEngine;

public class ArrowPower : PowerController
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

    protected virtual void Aim(float angle)
    {

    }

    protected virtual void Shoot(Vector3 vector)
    {

    }

    protected void CreateArrow(Vector3 vector, Rigidbody2D arrow, Vector3 position)
    {
        Rigidbody2D shotRigid = Instantiate(arrow, position, aimingPoint.rotation);
        Vector3 force = -1f * arrowSpeed * vector;
        shotRigid.velocity = force;

        OnShotInstantiated(shotRigid, damage);
    }
}
