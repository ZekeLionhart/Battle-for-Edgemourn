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
        Vector3 force = 1.73f * (new Vector3(4f, 4f, 0f) - transform.position);
        shotRigid.velocity = force;

        OnBoulderInstantiated(shotRigid, damage);
    }

    private void PlayReloadSfx()
    {
        if (onReloadSfx != null)
            onReloadSfx.Play();
    }
}