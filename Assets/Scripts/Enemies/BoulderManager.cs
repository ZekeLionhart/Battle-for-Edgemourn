using UnityEngine;

public class BoulderManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private AudioSource hitSFX;
    private int damage;
    private float shakeDuration;
    private float shakeIntensity;
    private bool hasGameEnded = false;

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
        if (collision.collider.CompareTag(TagNames.Tower) || collision.collider.CompareTag(TagNames.Player))
        {
            animator.SetTrigger(ParameterNames.Destroy);
            Destroy(rigidBody);

            if (hasGameEnded) return;

            if (collision.collider.TryGetComponent<IDamageable>(out IDamageable receiver))
                receiver.TakeDamage(damage);

            hitSFX.Play();
            CameraShake.CallShake(shakeDuration, shakeIntensity);
        }
    }

    public void Initialize(int damage, float shakeDuration, float shakeIntensity, Vector3 velocity)
    {
        this.damage = damage;
        this.shakeDuration = shakeDuration;
        this.shakeIntensity = shakeIntensity;
        rigidBody.velocity = velocity;
    }

    private void StopGame()
    {
        hasGameEnded = true;
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
