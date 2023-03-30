using UnityEngine;

public class BoilingOilController : PowerController
{
    [SerializeField] private float speed;
    
    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && canShoot)
            Shoot();
    }

    protected override void Shoot()
    {
        Rigidbody2D shotRigid = Instantiate(shot, new Vector2(
                aimingPoint.transform.position.x, 
                aimingPoint.transform.position.y
            ), shot.transform.rotation);

        OnShotInstantiated(shotRigid.gameObject, damageType, damage, speed);
        base.Shoot();
    }
}
