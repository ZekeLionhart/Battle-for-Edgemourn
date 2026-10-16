using UnityEngine;
using Random = UnityEngine.Random;

public class ArrowManager : ProjectileManager
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private AudioSource arrowLooseSfx;
    [SerializeField] private AudioSource groundHitSfx;
    [SerializeField] private float lifespan;
    [SerializeField] private TrailRenderer trailRenderer;
    private bool canDamage = true;
    private bool canRotate = true;

    private void Awake()
    {
        arrowLooseSfx.pitch = Random.Range(0.9f, 1.1f);
        arrowLooseSfx.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!canDamage) return;

        canDamage = false;

        Vector2 hitPoint = collision.GetContact(0).point;

        rigidBody.simulated = false;
        canRotate = false;
        trailRenderer.enabled = false;

        Destroy(gameObject, lifespan);

        if (collision.collider.CompareTag(TagNames.Floor))
        {
            groundHitSfx.pitch = Random.Range(0.9f, 1.1f);
            groundHitSfx.Play();
        }

        bool shouldDamage = true;

        if (collision.collider.TryGetComponent<IArrowHitReceiver>(out IArrowHitReceiver receiver))
            shouldDamage = receiver.PinArrow(hitPoint, transform.rotation, rigidBody);
        else
            transform.SetParent(collision.collider.transform, true);
            
        if (shouldDamage && collision.collider.TryGetComponent<IDamageableEnemy>(out IDamageableEnemy damageable))
            damageable.TakeDamage(gameObject, powerType, damageType, damage);
    }

    private void FixedUpdate()
    {
        if (!PauseManager.isPaused && canRotate)
            RotateMidFlight();
    }

    public void Launch(Vector3 velocity)
    {
        rigidBody.velocity = velocity;
    }

    private void RotateMidFlight()
    {
        Vector2 dir = rigidBody.velocity;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
