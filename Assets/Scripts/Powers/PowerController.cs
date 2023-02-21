using System;
using System.Collections;
using UnityEngine;

public class PowerController : MonoBehaviour
{
    [SerializeField] protected Transform aimingPoint;
    [SerializeField] protected Rigidbody2D shot;
    [SerializeField] protected float damage;
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
        CooldownManager.OnCooldownEnded += AllowShooting;
    }

    protected virtual void OnDisable()
    {
        CooldownManager.OnCooldownEnded -= AllowShooting;
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
