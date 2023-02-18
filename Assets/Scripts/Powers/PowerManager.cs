using System;
using System.Collections;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PowerManager : MonoBehaviour
{
    [SerializeField] protected Transform aimingPoint;
    [SerializeField] protected Rigidbody2D arrow;
    [SerializeField] protected float arrowSpeed;
    [SerializeField] protected float cooldown;
    protected bool isActive;
    protected bool canShoot = true;
    protected WaitForSeconds shotDelayWFS;

    public static Action<PowerManager, float> OnPowerShoot;

    protected virtual void Awake()
    {
        shotDelayWFS = new WaitForSeconds(cooldown);
    }

    private void OnEnable()
    {
        LineDrawer.OnMouseUp += Shoot;
        LineDrawer.OnAimUpdate += Aim;
    }

    private void OnDisable()
    {
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
        Vector3 force = vector * arrowSpeed * -1f;
        shotRigid.velocity = force;
    }

    protected IEnumerator ShotDelay()
    {
        canShoot = false;

        yield return shotDelayWFS;

        canShoot = true;
    }
}
