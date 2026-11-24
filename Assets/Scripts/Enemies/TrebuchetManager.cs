using UnityEngine;

public class TrebuchetManager : EnemyBase
{
    [Header("--Individual Fields--")]
    [SerializeField] private Transform ammoSpawn;
    [SerializeField] private AudioSource onReloadSfx;

    private ProjectilePool projectilePool;

    protected override void Awake()
    {
        base.Awake();
        bonusWindow = 10f;
    }

    public void SetProjectilePool(ProjectilePool projectilePool)
    {
        this.projectilePool = projectilePool;
    }

    protected override void Attack()
    {
        if (hasGameEnded) return;

        BoulderManager boulder = projectilePool.GetBoulder();
        boulder.transform.SetPositionAndRotation(ammoSpawn.position, ammoSpawn.rotation);

        Vector3 velocity = CalculateBallisticVelocity(new Vector2(-6.5f, -0.5f));
        boulder.Initialize(damage, shakeDuration, shakeIntensity, velocity);
    }

    private void PlayReloadSfx()
    {
        if (onReloadSfx == null) return;

        onReloadSfx.Play();
    }

    private Vector3 CalculateBallisticVelocity(Vector3 targetPosition)
    {
        Vector3 dir = targetPosition - transform.position; // get target direction
        dir.y = 0f;  // retain only the horizontal direction
        float dist = dir.magnitude;  // get horizontal distance
        dir.y = dist;  // set elevation to 45 degrees
        float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude);
	    return vel * dir.normalized;  // returns Vector3 velocity
    }

    protected override void ReturnToPool()
    {
        onReloadSfx.Stop();
        base.ReturnToPool();
    }
}