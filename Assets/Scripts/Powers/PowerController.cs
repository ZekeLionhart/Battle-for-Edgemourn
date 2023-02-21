using System;
using System.Collections;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PowerController : MonoBehaviour
{
    [SerializeField] protected Transform aimingPoint;
    [SerializeField] protected Rigidbody2D shot;
    [SerializeField] protected float damage;
    [SerializeField] protected float arrowSpeed;
    [SerializeField] protected float cooldown;
    protected bool isActive;
    protected bool canShoot = true;
    protected WaitForSeconds shotDelayWFS;

    public static Action<PowerController, float> OnPowerShoot;
    public static Action<Rigidbody2D, float> OnShotInstantiated;

    protected virtual void Awake()
    {
        shotDelayWFS = new WaitForSeconds(cooldown);
    }

    protected virtual void OnEnable()
    {
        LineDrawer.OnMouseUp += Shoot;
        LineDrawer.OnAimUpdate += Aim;
        CooldownManager.OnCooldownEnded += AllowShooting;
    }

    protected virtual void OnDisable()
    {
        LineDrawer.OnMouseUp -= Shoot;
        LineDrawer.OnAimUpdate -= Aim;
        CooldownManager.OnCooldownEnded -= AllowShooting;
    }

    protected virtual void Aim(float angle)
    {

    }

    protected virtual void Shoot()
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

    protected void AllowShooting(PowerController power)
    {
        if (power == this)
            canShoot = true;
    }

    protected IEnumerator ShotDelay()
    {
        canShoot = false;

        yield return shotDelayWFS;

        canShoot = true;
    }
}
