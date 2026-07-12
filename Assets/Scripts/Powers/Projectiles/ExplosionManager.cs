using UnityEngine;

public class ExplosionManager : ProjectileManager
{
    [SerializeField] private Transform burnMark;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private CircleCollider2D explosionCollider;
    private readonly float rayDistance = 10f;

    private void Awake()
    {
        PlaceBurnMark();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagNames.Enemy))
            OnEnemyHit(gameObject, collision.gameObject, powerType, damageType, damage);
    }

    private void PlaceBurnMark()
    {
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

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}