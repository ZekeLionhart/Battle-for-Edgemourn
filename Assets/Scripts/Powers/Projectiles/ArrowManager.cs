using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class ArrowManager : ProjectileManager
{
    private enum ArrowState
    {
        Stored,
        Flying,
        Deflected,
        Pinned
    }

    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private CapsuleCollider2D defaultCollider;
    [SerializeField] private CapsuleCollider2D deflectCollider;
    [SerializeField] private AudioSource arrowLooseSfx;
    [SerializeField] private AudioSource groundHitSfx;
    [SerializeField] private float lifespan;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField, Min(0.1f)] private float maxFlightTime = 15f;

    private IObjectPool<ArrowManager> pool;
    private ArrowState state;
    private Transform pinTarget;
    private Vector3 pinLocalPosition;
    private Quaternion pinLocalRotation;
    private float returnTime;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (state != ArrowState.Flying) return;

        state = ArrowState.Pinned;
        rigidBody.simulated = false;
        trailRenderer.enabled = false;
        returnTime = Time.time + lifespan;

        Vector2 hitPoint = collision.GetContact(0).point;

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

        // Blocked arrows may return here
        if (state != ArrowState.Pinned) return;

        pinTarget = transform.parent;
        pinLocalPosition = transform.localPosition;
        pinLocalRotation = transform.localRotation;
        transform.SetParent(null, true);

        if (shouldDamage && collision.collider.TryGetComponent<IDamageableEnemy>(out IDamageableEnemy damageable))
            damageable.TakeDamage(gameObject, powerType, damageType, damage);
    }

    private void FixedUpdate()
    {
        if (PauseManager.isPaused) return;

        if (state != ArrowState.Flying && state != ArrowState.Deflected) return;

        if (Time.time >= returnTime)
        {
            ReturnToPool();
            return;
        }

        if (state == ArrowState.Flying)
            RotateMidFlight();
    }

    private void LateUpdate()
    {
        if (state != ArrowState.Pinned) return;

        if (pinTarget == null || Time.time >= returnTime)
        {
            ReturnToPool();
            return;
        }

        transform.SetPositionAndRotation(
            pinTarget.TransformPoint(pinLocalPosition),
            pinTarget.rotation * pinLocalRotation);
    }

    public void SetPool(IObjectPool<ArrowManager> pool)
    {
        this.pool = pool;
    }

    public void Launch(Vector3 velocity)
    {
        state = ArrowState.Flying;
        returnTime = Time.time + maxFlightTime;
        gameObject.SetActive(true);

        defaultCollider.enabled = true;
        deflectCollider.enabled = false;

        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        rigidBody.simulated = true;
        rigidBody.linearVelocity = velocity;
        rigidBody.angularVelocity = 0f;

        trailRenderer.enabled = true;
        trailRenderer.Clear();

        arrowLooseSfx.pitch = Random.Range(0.9f, 1.1f);
        arrowLooseSfx.Play();
    }

    public void ReturnToPool()
    {
        if (state == ArrowState.Stored) return;

        state = ArrowState.Stored;
        pinTarget = null;

        rigidBody.simulated = false;
        trailRenderer.enabled = false;
        arrowLooseSfx.Stop();
        groundHitSfx.Stop();

        pool.Release(this);
    }

    private void RotateMidFlight()
    {
        Vector2 dir = rigidBody.linearVelocity;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Deflect(float rand)
    {
        state = ArrowState.Deflected;

        trailRenderer.enabled = false;

        defaultCollider.enabled = false;
        deflectCollider.enabled = true;

        rigidBody.constraints = RigidbodyConstraints2D.None;
        rigidBody.simulated = true;
        rigidBody.linearVelocity = new Vector2(2f, 3f);
        rigidBody.angularVelocity = -360f * (rand * 5);
    }
}
