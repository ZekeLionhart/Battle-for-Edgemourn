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
    [SerializeField] protected float shakeDuration;
    [SerializeField] protected float shakeIntensity;
    protected bool isActive;
    protected bool isMouseInside;
    protected bool offCooldown = true;
    protected WaitForSeconds shotDelayWFS;

    public static Action<PowerController, float> OnPowerShoot;
    public static Action<GameObject, PowerTypes, DamageTypes, float, float, float, float> OnShotInstantiated;

    protected virtual void Awake()
    {
        shotDelayWFS = new WaitForSeconds(cooldown);
    }

    protected virtual void OnEnable()
    {
        PowerManager.OnSwitchPowers += DeActivate;
        CooldownManager.OnCooldownEnded += AllowShooting;
        HealthManager.OnZeroHealth += CeaseFunction;
    }

    protected virtual void OnDisable()
    {
        PowerManager.OnSwitchPowers -= DeActivate;
        CooldownManager.OnCooldownEnded -= AllowShooting;
        HealthManager.OnZeroHealth -= CeaseFunction;
    }

    protected void AllowShooting(PowerController power)
    {
        if (power == this)
            offCooldown = true;
    }

    protected bool TryShoot()
    {
        if (Input.GetButtonUp(KeyNames.Fire) && offCooldown && isMouseInside && isActive) return true;
        return false;
    }

    protected virtual void Shoot()
    {
        CallShotAnalytics();
        OnPowerShoot(this, cooldown);
        offCooldown = false;
        isMouseInside = false;
    }

    protected IEnumerator ShotDelay()
    {
        offCooldown = false;

        yield return shotDelayWFS;

        offCooldown = true;
    }

    public void SetUpStartingPower(PowerController activePower)
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
        else if (isActive)
        {
            isActive = false;
            content.SetActive(false);
            if (idleSfx != null)
                idleSfx.Stop();
        }
    }

    private void CeaseFunction()
    {
        offCooldown = false;
    }

    protected void CallShotAnalytics()
    {
        Analytics.OnPowerUsed(powerType);
    }
}
