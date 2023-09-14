using UnityEngine;

public class ArrowManager : ProjectileManager
{
    [SerializeField] private AudioSource groundHitSfx;
    [SerializeField] private float lifespan;
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
        rigid.simulated = false;


        GameObject target = collision.collider.gameObject;

        if (target.transform.Find("Bones (spine)") != null)
            target = target.transform.Find("Bones (spine)").gameObject;
        else if (target.transform.Find("Roof") != null)
            target = target.transform.Find("Roof").gameObject;
        else if (target.transform.Find("bone_1") != null)
            target = target.transform.Find("bone_1").gameObject;
        if (target != collision.collider.gameObject)


        transform.parent = target.transform;
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
