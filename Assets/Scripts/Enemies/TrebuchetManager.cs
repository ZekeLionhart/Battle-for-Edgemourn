using System;
using UnityEngine;

public class TrebuchetManager : EnemyBase
{
    [SerializeField] private Rigidbody2D ammo;

    public static Action<Rigidbody2D, int> OnBoulderInstantiated;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TrebLimit"))
            animator.SetBool("IsColliding", true);
    }

    protected override void Attack()
    {
        Rigidbody2D shotRigid = Instantiate(ammo, transform.position, transform.rotation);
        Vector3 force = 2f * (new Vector3(4f, 4f, 0f) - transform.position);
        shotRigid.velocity = force;

        OnBoulderInstantiated(shotRigid, damage);
    }
}