using System;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    protected PowerTypes powerType;
    protected DamageTypes damageType;
    protected float damage;
    protected float speed;
    protected float shakeDuration;
    protected float shakeIntensity;

    public static Action<GameObject, GameObject, PowerTypes, DamageTypes, float> OnEnemyHit;

    public void Initialize(PowerTypes powerType, DamageTypes damageType, float damage, float speed, float shakeDuration, float shakeIntensity)
    {
        this.powerType = powerType;
        this.damageType = damageType;
        this.damage = damage;
        this.speed = speed;
        this.shakeDuration = shakeDuration;
        this.shakeIntensity = shakeIntensity;
    }

    private void CallCameraShake()
    {
        CameraShake.CallShake(shakeDuration, shakeIntensity);
    }
}
