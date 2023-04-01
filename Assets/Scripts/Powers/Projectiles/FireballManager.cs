using System;
using UnityEngine;

public class FireballManager : ProjectileManager
{
    public static Action<Vector3> OnTargetHit;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor") 
            || collision.collider.CompareTag("Enemy") 
            || collision.collider.CompareTag("Player")) 
        {
            OnTargetHit(transform.position);
            Destroy(gameObject);
        }
    }
}
