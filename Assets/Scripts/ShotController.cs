using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float drag;

    public static Action<GameObject> OnEnemyHit;

    private void Start()
    {
        SetupSpeed();
    }

    private void FixedUpdate()
    {
        transform.Translate(Time.fixedDeltaTime * speed * transform.right, Space.World);
        speed -= drag;
        if (speed < 0)
            speed = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag.Equals("Enemy"))
            OnEnemyHit(collision.collider.gameObject);

        Destroy(this.gameObject);
    }

    private void SetupSpeed()
    {
        speed *= 2f;
        if (speed > maxSpeed)
            speed = maxSpeed;
        if (speed < 2f)
            speed = 2f;
    }
}
