using UnityEngine;

public class BowController : PowerController
{
    [SerializeField] protected float arrowSpeed;

    protected override void OnEnable()
    {
        base.OnEnable();
        LineDrawer.OnMouseUp += Shoot;
        LineDrawer.OnAimUpdate += Aim;
        LineDrawer.OnBowPull += PullBow;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        LineDrawer.OnMouseUp -= Shoot;
        LineDrawer.OnAimUpdate -= Aim;
        LineDrawer.OnBowPull -= PullBow;
    }

    private void Aim(float angle)
    {
        aimingPoint.rotation = Quaternion.identity;
        aimingPoint.Rotate(0, 0, angle);
    }

    protected virtual void Shoot(Vector3 vector)
    {
        animator.SetTrigger("Shoot");

        if (canShoot)
            CreateArrow(vector, shot, aimingPoint.position);
    }

    protected void CreateArrow(Vector3 vector, Rigidbody2D arrow, Vector3 position)
    {
        Rigidbody2D shotRigid = Instantiate(arrow, position, aimingPoint.rotation);
        Vector3 force = -1f * arrowSpeed * vector;
        shotRigid.velocity = force;

        OnShotInstantiated(shotRigid.gameObject, damageType, damage, 0f);
        OnPowerShoot(this, cooldown);
        canShoot = false;
    }

    private void PullBow(int strength)
    {
        switch(strength)
        {
            case 1:
                animator.SetTrigger("PullWeak");
                break;
            case 2:
                animator.SetTrigger("PullMed");
                break;
            case 3:
                animator.SetTrigger("PullStrong");
                break;
            default:
                break;
        }
    }
}
