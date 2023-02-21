using System;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    protected float damage;

    public static Action<GameObject, float> OnEnemyHit;
    protected virtual void OnEnable()
    {
        PowerController.OnShotInstantiated += SetDamage;
    }

    protected virtual void OnDisable()
    {
        PowerController.OnShotInstantiated -= SetDamage;
    }

    private void SetDamage(Rigidbody2D shot, float damage)
    {
        if (shot.gameObject == gameObject)
            this.damage = damage;
    }
}
