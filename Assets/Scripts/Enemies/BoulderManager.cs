using UnityEngine;

public class BoulderManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private AudioSource hitSFX;
    private int damage;
    private float shakeDuration;
    private float shakeIntensity;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(TagNames.Tower) || collision.collider.CompareTag(TagNames.Player))
        {
            EnemyBase.OnDamageDealt(collision.gameObject, damage);
            animator.SetTrigger(ParameterNames.Destroy);
            Destroy(rigidBody);
        }
    }

    public void Initialize(int damage, float shakeDuration, float shakeIntensity, Vector3 velocity)
    {
        this.damage = damage;
        this.shakeDuration = shakeDuration;
        this.shakeIntensity = shakeIntensity;
        rigidBody.velocity = velocity;
    }

    private void CallCameraShake()
    {
        CameraShake.CallShake(shakeDuration, shakeIntensity);
    }

    public void PlayHitSound()
    {
        hitSFX.Play();
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
