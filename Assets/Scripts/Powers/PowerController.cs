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
    private bool hasStopped;
    private Coroutine cooldownRoutine;

    public float CooldownProgress { get; private set; } = 1f;

    public static Action<PowerController, float> OnPowerShoot;

    protected virtual void OnEnable()
    {
        PowerManager.OnSwitchPowers += DeActivate;
        HealthManager.OnZeroHealth += CeaseFunction;
    }

    protected virtual void OnDisable()
    {
        PowerManager.OnSwitchPowers -= DeActivate;
        HealthManager.OnZeroHealth -= CeaseFunction;
    }

    protected bool TryShoot()
    {
        if (Input.GetButtonUp(KeyNames.Fire) && offCooldown && isMouseInside && isActive) return true;
        return false;
    }

    protected virtual void Shoot()
    {
        StartCooldown();
        isMouseInside = false;

        CallShotAnalytics();
        OnPowerShoot(this, cooldown);
    }

    protected void StartCooldown()
    {
        if (hasStopped) return;

        if (cooldownRoutine != null)
            StopCoroutine(cooldownRoutine);

        offCooldown = false;
        CooldownProgress = 0f;

        cooldownRoutine = StartCoroutine(RunCooldown());
    }

    private IEnumerator RunCooldown()
    {
        float duration = cooldown;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            yield return null;

            elapsed += Time.deltaTime;
            CooldownProgress = Mathf.Clamp01(elapsed / duration);
        }

        CooldownProgress = 1f;
        offCooldown = true;
        cooldownRoutine = null;
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
        hasStopped = true;
        offCooldown = false;

        if (cooldownRoutine != null)
        {
            StopCoroutine(cooldownRoutine);
            cooldownRoutine = null;
        }
    }

    protected void CallShotAnalytics()
    {
        Analytics.OnPowerUsed(powerType);
    }
}
