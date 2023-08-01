using UnityEngine;

public class ArrowManager : ProjectileManager
{
    [SerializeField] private AudioSource groundHitSfx;
    private bool canDamage = true;
    private bool canRotate = true;
    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") && canDamage)
        {
            OnEnemyHit(collision.collider.gameObject, damageType, damage);
            canDamage = false;
        }

        if (collision.collider.CompareTag("Floor"))
            groundHitSfx.Play();

        canRotate = false;
        GetComponent<Rigidbody2D>().simulated = false;
        Destroy(gameObject, 1f);
    }

    private void Update()
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
