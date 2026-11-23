using UnityEngine;
using UnityEngine.Pool;

public class BoulderManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private AudioSource hitSFX;
    [SerializeField] private ParticleSystem dustParticles;

    private IObjectPool<BoulderManager> pool;
    private int damage;
    private float shakeDuration;
    private float shakeIntensity;
    private bool hasGameEnded;
    private bool inFlight;

    private void Awake()
    {
        animator.writeDefaultValuesOnDisable = true;//???
    }

    private void OnEnable()
    {
        GameManager.OnGameEnded += StopGame;
    }

    private void OnDisable()
    {
        GameManager.OnGameEnded -= StopGame;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!inFlight) return;

        Collider2D hit = collision.collider;

        if (!hit.CompareTag(TagNames.Tower) && !hit.CompareTag(TagNames.Player))
            return;

        inFlight = false;
        rigidBody.simulated = false;
        animator.SetTrigger(ParameterNames.Destroy);

        if (hasGameEnded) return;

        if (collision.collider.TryGetComponent<IDamageable>(out IDamageable receiver))
            receiver.TakeDamage(damage);

        hitSFX.Play();
        CameraShake.Instance.Shake(shakeDuration, shakeIntensity);
    }

    public void Initialize(int damage, float shakeDuration, float shakeIntensity, Vector3 velocity)
    {
        this.damage = damage;
        this.shakeDuration = shakeDuration;
        this.shakeIntensity = shakeIntensity;

        hasGameEnded = false;
        inFlight = true;

        gameObject.SetActive(true);
        animator.Update(0f);

        rigidBody.simulated = true;
        rigidBody.linearVelocity = velocity;
        rigidBody.angularVelocity = 0f;
    }

    public void SetPool(IObjectPool<BoulderManager> pool)
    {
        this.pool = pool;
    }

    private void StopGame()
    {
        hasGameEnded = true;
    }

    private void ReturnToPool()
    {
        inFlight = false;
        rigidBody.simulated = false;
        dustParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        hitSFX.Stop();

        pool.Release(this);
    }
}
