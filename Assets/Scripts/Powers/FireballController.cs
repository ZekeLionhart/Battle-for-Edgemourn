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

        if (Input.GetButtonDown("Fire1") && canShoot)
            Shoot();
    }

    protected override void Shoot()
    {
        Rigidbody2D shotRigid = Instantiate(shot, new Vector2(transform.position.x, transform.position.y)
            , aimingPoint.transform.rotation);
        Vector3 force = speed * (direction.position - aimingPoint.position);
        shotRigid.velocity = force;

        OnShotInstantiated(shotRigid.gameObject, damageType, damage, speed);
        base.Shoot();
    }

    private void InvokeExplosion(Vector3 position)
    {
        GameObject newShot = Instantiate(explosion, position, Quaternion.identity);
        OnShotInstantiated(newShot, damageType, damage, speed);
    }
}
