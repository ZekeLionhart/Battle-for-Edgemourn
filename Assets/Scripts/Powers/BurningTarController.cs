using UnityEngine;

public class BurningTarController : PowerController
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject particles;
    
    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && canShoot && isActive)
        {
            animator.SetTrigger("Flip");
            Invoke(nameof(Shoot), 0.5f);
        }
    }

    protected override void Shoot()
    {
        Rigidbody2D shotRigid = Instantiate(shot, new Vector2(
                aimingPoint.transform.position.x + 0.4f,
                aimingPoint.transform.position.y
            ), shot.transform.rotation);

        Instantiate(particles, new Vector2(
                aimingPoint.transform.position.x,
                aimingPoint.transform.position.y), 
            aimingPoint.rotation);

        OnShotInstantiated(shotRigid.gameObject, damageType, damage, speed);
        base.Shoot();
    }
}
