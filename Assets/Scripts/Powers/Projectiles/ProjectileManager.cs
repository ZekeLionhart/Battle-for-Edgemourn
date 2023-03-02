using System;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    protected DamageTypes damageType;
    protected float damage;
    protected float speed;

    public static Action<GameObject, DamageTypes, float> OnEnemyHit;
    
    protected virtual void OnEnable()
    {
        PowerController.OnShotInstantiated += SetVariables;
    }

    protected virtual void OnDisable()
    {
        PowerController.OnShotInstantiated -= SetVariables;
    }

    private void SetVariables(GameObject shot, DamageTypes damageType, float damage, float speed)
    {
        if (shot == gameObject)
        {
            this.damageType = damageType;
            this.damage = damage;
            this.speed = speed;
        }
    }
}
