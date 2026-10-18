using UnityEngine;

public class WallManager : MonoBehaviour, IDamageable
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource hitSfx;
    private int hitpoints = 100;
    private float damage;
    private float shakeDuration;
    private float shakeIntensity;
    private bool canBeDamaged = false;
    private PowerTypes powerType;
    private DamageTypes damageType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IDamageableEnemy>(out IDamageableEnemy receiver))
            receiver.TakeDamage(gameObject, powerType, damageType, damage);
    }

    public void Initialize(int hitpoints, float duration, float damage, PowerTypes powerType, DamageTypes damageType, float shakeDuration, float shakeIntensity)
    {
        this.powerType = powerType;
        this.hitpoints = hitpoints;
        this.damage = damage;
        this.damageType = damageType;
        this.shakeDuration = shakeDuration;
        this.shakeIntensity = shakeIntensity;
        Invoke(nameof(ForceDeath), duration);
    }

    public void TakeDamage(int damage)
    {
        if (hitpoints <= 0) return;

        hitSfx.Play();

        if (!canBeDamaged) return;

        hitpoints -= damage;

        if (hitpoints <= 0)
            animator.SetTrigger(ParameterNames.Destroy);
        else
            animator.SetTrigger(ParameterNames.Hit);
    }

    private void SummoningEnded()
    {
        canBeDamaged = true;
    }

    private void CallCameraShake()
    {
        CameraShake.Instance.Shake(shakeDuration, shakeIntensity);
    }

    private void ForceDeath()
    {
        TakeDamage(1000);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
