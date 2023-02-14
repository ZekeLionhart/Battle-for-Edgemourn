using System;
using System.Collections;
using UnityEngine;

public class BowManager : MonoBehaviour
{
    [SerializeField] private ShotController arrow;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private Transform aimingPoint;
    [SerializeField] private float cooldown;
    private bool canShoot = true;
    private WaitForSeconds shotDelayWFS;

    public static Action<float> OnBowShoot;

    private void Awake()
    {
        shotDelayWFS = new WaitForSeconds(cooldown);
    }

    private void OnEnable()
    {
        LineDrawer.OnMouseUp += Shoot;
        LineDrawer.OnAimUpdate += Aim;
    }

    private void OnDisable()
    {
        LineDrawer.OnMouseUp -= Shoot;
        LineDrawer.OnAimUpdate -= Aim;
    }

    private void Aim(float angle)
    {
        aimingPoint.rotation = Quaternion.identity;
        aimingPoint.Rotate(0, 0, angle);
    }

    private void Shoot(Vector3 vector)
    {
        if (canShoot)
        {
            Rigidbody2D shotRigid = Instantiate(arrow, aimingPoint.position, aimingPoint.rotation).GetComponent<Rigidbody2D>();
            Vector3 force = vector * arrowSpeed * -1f;
            shotRigid.velocity = force;

            OnBowShoot(cooldown);
            StartCoroutine(ShootDelay());
        }
    }

    private IEnumerator ShootDelay()
    {
        canShoot = false;

        yield return shotDelayWFS;

        canShoot = true;
    }
}
