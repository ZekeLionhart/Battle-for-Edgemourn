using UnityEngine;

public class FireballController : CrosshairPower
{
    [SerializeField] private float speed;
    [SerializeField] private Transform direction;

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

        OnShotInstantiated(shotRigid, damageType, damage, speed);
        base.Shoot();
    }
}
