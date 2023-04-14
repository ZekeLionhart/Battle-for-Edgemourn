using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

public class TrebuchetManager : EnemyBase
{
    [SerializeField] private Rigidbody2D ammo;
    [SerializeField] private Transform ammoSpawn;

    public static Action<Rigidbody2D, int> OnBoulderInstantiated;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TrebLimit"))
            animator.SetBool("IsColliding", true);
    }

    protected override void CalculateDistanceToTarget()
    {
        
    }

    protected override void Attack()
    {
        Rigidbody2D shotRigid = Instantiate(ammo, ammoSpawn.position, ammoSpawn.rotation);
        Vector3 force = 1.73f * (new Vector3(4f, 4f, 0f) - transform.position);
        shotRigid.velocity = force;

        OnBoulderInstantiated(shotRigid, damage);
    }
}