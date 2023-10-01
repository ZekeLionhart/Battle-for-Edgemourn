using System;
using UnityEngine;

public class ArrowManager : ProjectileManager
{
    [SerializeField] private AudioSource groundHitSfx;
    [SerializeField] private float lifespan;
    [SerializeField] private TrailRenderer trailRenderer;
    private bool canDamage = true;
    private bool canRotate = true;
    private Rigidbody2D rigid;

    public static Action<GameObject, Rigidbody2D, DamageTypes, float> OnEnemyHitWithArrow;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(TagNames.Enemy) && canDamage)
        {
            OnEnemyHitWithArrow(collision.collider.gameObject, rigid, damageType, damage);
            canDamage = false;
        }

        if (collision.collider.CompareTag(TagNames.Floor))
            groundHitSfx.Play();

        rigid.simulated = false;
        canRotate = false;
        trailRenderer.enabled = false;

        Destroy(gameObject, lifespan);
    }

    private void FixedUpdate()
    {
        if (!PauseManager.isPaused && canRotate)
            RotateMidFlight();
    }

    private void RotateMidFlight()
    {
        Vector2 dir = rigid.velocity;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
