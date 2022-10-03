using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] private float drag;

    public static Action<GameObject> OnEnemyHit;

    private void FixedUpdate()
    {
        transform.position += transform.right * speed * Time.fixedDeltaTime;
        speed -= drag;
        if (speed < 0)
            speed = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
            OnEnemyHit(collision.collider.gameObject);

        Destroy(this.gameObject);
    }
}
