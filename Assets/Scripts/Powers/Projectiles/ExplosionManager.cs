using UnityEngine;
using UnityEngine.Pool;

public class ExplosionManager : ProjectileManager
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform burnMark;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private CircleCollider2D explosionCollider;
    [SerializeField] private ParticleSystem[] particles;

    private readonly float rayDistance = 10f;

    private IObjectPool<ExplosionManager> pool;
    private Vector3 burnMarkLocalPosition;
    private Vector3 burnMarkLocalScale;

    private void Awake()
    {
        if (!burnMark) return;

        burnMarkLocalPosition = burnMark.localPosition;
        burnMarkLocalScale = burnMark.localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageableEnemy>(out IDamageableEnemy receiver))
            receiver.TakeDamage(gameObject, powerType, damageType, damage);
    }

    public void SetPool(IObjectPool<ExplosionManager> pool)
    {
        this.pool = pool;
    }

    public void Play()
    {
        gameObject.SetActive(true);
        animator.Update(0f);

        if (burnMark)
        {
            burnMark.localPosition = burnMarkLocalPosition;
            burnMark.localScale = burnMarkLocalScale;
        }

        PlaceBurnMark();
    }

    private void PlaceBurnMark()
    {
        if (!burnMark) return;

        Vector2 origin = burnMark.transform.position + Vector3.up; //gives some distance to avoid clipping

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, rayDistance, groundLayer);

        if (hit)
        {
            //Position
            burnMark.position = hit.point;

            //Scale
            float distance = hit.distance - explosionCollider.radius - 1f; //1f compensates for Vector3.up
            float scale = Mathf.Lerp(1f, 0f, distance / (rayDistance - explosionCollider.radius - 8f));
            burnMark.transform.localScale = Vector3.one * scale;
        }
    }

    private void ReturnToPool()
    {
        foreach (ParticleSystem particle in particles)
            particle.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);

        pool.Release(this);
    }
}
