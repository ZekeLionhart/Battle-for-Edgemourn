using System;
using UnityEngine;

public class TrebuchetManager : EnemyBase
{
    [SerializeField] private Rigidbody2D ammo;
    [SerializeField] private Transform ammoSpawn;
    [SerializeField] private AudioSource onReloadSfx;

    public static Action<Rigidbody2D, int> OnBoulderInstantiated;

    protected override void Attack()
    {
        Rigidbody2D shotRigid = Instantiate(ammo, ammoSpawn.position, ammoSpawn.rotation);
        //Vector3 force = 1.73f * (new Vector3(4f, 4f, 0f) - transform.position);
        //shotRigid.velocity = force;
        shotRigid.velocity = CalculateBallisticVelocity(new Vector2(-6.5f, -0.5f));

        OnBoulderInstantiated(shotRigid, damage);
    }

    private void PlayReloadSfx()
    {
        if (onReloadSfx != null)
            onReloadSfx.Play();
    }

    private Vector3 CalculateBallisticVelocity(Vector3 targetPosition)
    {
        Vector3 dir = targetPosition - transform.position; // get target direction
        dir.y = 0f;  // retain only the horizontal direction
        float dist = dir.magnitude;  // get horizontal distance
        dir.y = dist;  // set elevation to 45 degrees
        float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude);
	    return vel * dir.normalized;  // returns Vector3 velocity
    }
}