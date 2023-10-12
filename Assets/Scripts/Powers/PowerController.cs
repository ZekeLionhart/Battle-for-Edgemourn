using System;
using System.Collections;
using UnityEngine;

public class PowerController : MonoBehaviour
{
    [SerializeField] protected Transform aimingPoint;
    [SerializeField] protected GameObject content;
    [SerializeField] protected AudioSource wakeUpSfx;
    [SerializeField] protected AudioSource wakeUpSfx2;
    [SerializeField] protected AudioSource idleSfx;
    [SerializeField] protected Rigidbody2D shot;
    [SerializeField] protected Animator animator;
    [SerializeField] protected PowerTypes powerType;
    [SerializeField] protected DamageTypes damageType;
    [SerializeField] protected float damage;
    [SerializeField] protected float cooldown;
    protected bool isActive;
    protected bool canShoot = true;
    protected WaitForSeconds shotDelayWFS;

    public static Action<PowerController, float> OnPowerShoot;
    public static Action<GameObject, PowerTypes, DamageTypes, float, float> OnShotInstantiated;

    protected virtual void Awake()
    {
        shotDelayWFS = new WaitForSeconds(cooldown);
    }

    protected virtual void OnEnable()
    {
        PowerManager.SetUpStartingPower += SetUpStartingPower;
        PowerManager.OnSwitchPowers += DeActivate;
        CooldownManager.OnCooldownEnded += AllowShooting;
        HealthManager.OnZeroHealth += CeaseFunction;
        PowerShooter.OnScreenClick += AttemptShooting;
    }

    protected virtual void OnDisable()
    {
        PowerManager.SetUpStartingPower -= SetUpStartingPower;
        PowerManager.OnSwitchPowers -= DeActivate;
        CooldownManager.OnCooldownEnded -= AllowShooting;
        HealthManager.OnZeroHealth -= CeaseFunction;
        PowerShooter.OnScreenClick -= AttemptShooting;
    }

    protected void AllowShooting(PowerController power)
    {
        if (power == this)
            canShoot = true;
    }

    protected virtual void Shoot()
    {
        CallShotAnalytics();
        OnPowerShoot(this, cooldown);
        canShoot = false;
    }

    protected virtual void AttemptShooting()
    {
        if (canShoot && isActive)
            Shoot();
    }

    protected IEnumerator ShotDelay()
    {
        canShoot = false;

        yield return shotDelayWFS;

        canShoot = true;
    }

    private void SetUpStartingPower(PowerController activePower)
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

    private void DeActivate(PowerController activePower)
    {
        if (activePower == this)
        {
            isActive = true;
            content.SetActive(true);
            wakeUpSfx.Play();
            if (wakeUpSfx2 != null)
                wakeUpSfx2.Play();
            if (idleSfx != null)
                idleSfx.Play();
        }
        else
        {
            isActive = false;
            content.SetActive(false);
            if (idleSfx != null)
                idleSfx.Stop();
        }
    }

    private void CeaseFunction()
    {
        canShoot = false;
    }

    protected void CallShotAnalytics()
    {
        Analytics.OnPowerUsed(powerType);
    }
}
