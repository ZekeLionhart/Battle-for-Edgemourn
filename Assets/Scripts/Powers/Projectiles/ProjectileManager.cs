using System;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    protected string damageType;
    protected float damage;
    protected float speed;

    public static Action<GameObject, string, float> OnEnemyHit;
    
    protected virtual void OnEnable()
    {
        PowerController.OnShotInstantiated += SetVariables;
    }

    protected virtual void OnDisable()
    {
        PowerController.OnShotInstantiated -= SetVariables;
    }

    private void SetVariables(Rigidbody2D shot, string damageType, float damage, float speed)
    {
        if (shot.gameObject == gameObject)
        {
            this.damageType = damageType;
            this.damage = damage;
            this.speed = speed;
        }
    }
}
