using System;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    protected float damage;
    protected float speed;

    public static Action<GameObject, float> OnEnemyHit;
    
    protected virtual void OnEnable()
    {
        PowerController.OnShotInstantiated += SetVariables;
    }

    protected virtual void OnDisable()
    {
        PowerController.OnShotInstantiated -= SetVariables;
    }

    private void SetVariables(Rigidbody2D shot, float damage, float speed)
    {
        if (shot.gameObject == gameObject)
        {
            this.damage = damage;
            this.speed = speed;
        }
    }
}
