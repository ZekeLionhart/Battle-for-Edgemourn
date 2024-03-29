using System;
using UnityEngine;

public class FireballController : CrosshairPower
{
    [SerializeField] private float speed;
    [SerializeField] private Transform direction;
    [SerializeField] private GameObject explosion;

    protected override void OnEnable()
    {
        base.OnEnable();
        FireballManager.OnTargetHit += InvokeExplosion;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        FireballManager.OnTargetHit -= InvokeExplosion;
    }

    protected override void Update()
    {
        HandleAimRotation();
    }

    protected override void Shoot()
    {
        if (isActive)
        {
            Rigidbody2D shotRigid = Instantiate(shot, transform.position, aimingPoint.transform.rotation);
            Vector3 force = speed * (direction.position - aimingPoint.position);
            shotRigid.velocity = force;

            OnShotInstantiated(shotRigid.gameObject, powerType, damageType, damage, speed);
            base.Shoot();
        }
    }

    private void InvokeExplosion(Vector3 position)
    {
        GameObject newShot = Instantiate(explosion, position, Quaternion.identity);
        OnShotInstantiated(newShot, powerType, damageType, damage, speed);
    }
}
