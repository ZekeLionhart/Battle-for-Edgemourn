using System;
using System.Collections;
using UnityEngine;

public class PowerController : MonoBehaviour
{
    [SerializeField] protected Transform aimingPoint;
    [SerializeField] protected GameObject content;
    [SerializeField] protected Rigidbody2D shot;
    [SerializeField] protected Animator animator;
    [SerializeField] protected DamageTypes damageType;
    [SerializeField] protected float damage;
    [SerializeField] protected float cooldown;
    protected bool isActive;
    protected bool canShoot = true;
    protected WaitForSeconds shotDelayWFS;

    public static Action<PowerController, float> OnPowerShoot;
    public static Action<GameObject, DamageTypes, float, float> OnShotInstantiated;

    protected virtual void Awake()
    {
        shotDelayWFS = new WaitForSeconds(cooldown);
    }

    protected virtual void OnEnable()
    {
        PowerManager.OnSwitchPowers += DeActivate;
        CooldownManager.OnCooldownEnded += AllowShooting;
    }

    protected virtual void OnDisable()
    {
        PowerManager.OnSwitchPowers -= DeActivate;
        CooldownManager.OnCooldownEnded -= AllowShooting;
    }

    protected void AllowShooting(PowerController power)
    {
        if (power == this)
            canShoot = true;
    }

    protected virtual void Shoot()
    {
        OnPowerShoot(this, cooldown);
        canShoot = false;
    }

    protected IEnumerator ShotDelay()
    {
        canShoot = false;

        yield return shotDelayWFS;

        canShoot = true;
    }

    private void DeActivate(PowerController activePower)
    {
        if (activePower == this)
        {
            isActive = true;
            content.SetActive(true);
        }
        else
        {
            isActive = false;
            content.SetActive(false);
        }
    }
}
