using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArrowManager : ProjectileManager
{
    [SerializeField] private AudioSource arrowLooseSfx;
    [SerializeField] private AudioSource groundHitSfx;
    [SerializeField] private float lifespan;
    [SerializeField] private TrailRenderer trailRenderer;
    private bool canDamage = true;
    private bool canRotate = true;
    private Rigidbody2D rigid;

    public static Action<GameObject, GameObject, Vector2, Quaternion, Rigidbody2D, PowerTypes, DamageTypes, float> OnEnemyHitWithArrow;

    private void Awake()
    {
        arrowLooseSfx.pitch = Random.Range(0.9f, 1.1f);
        arrowLooseSfx.Play();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(TagNames.Enemy) && canDamage)
        {
            Vector2 hitPoint = collision.GetContact(0).point;
            OnEnemyHitWithArrow(gameObject, collision.collider.gameObject, hitPoint, this.transform.rotation
                , rigid, powerType, damageType, damage);
            canDamage = false;
        }

        if (collision.collider.CompareTag(TagNames.Floor))
        {
            groundHitSfx.pitch = Random.Range(0.9f, 1.1f);
            groundHitSfx.Play();
        }

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
