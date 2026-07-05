using UnityEngine;

public class BoulderManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private AudioSource hitSFX;
    private int damage;
    private float shakeDuration;
    private float shakeIntensity;

    protected virtual void OnEnable()
    {
        TrebuchetManager.OnBoulderInstantiated += SetVariables;
    }

    protected virtual void OnDisable()
    {
        TrebuchetManager.OnBoulderInstantiated -= SetVariables;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(TagNames.Tower) || collision.collider.CompareTag(TagNames.Player))
        {
            EnemyBase.OnDamageDealt(collision.gameObject, damage);
            animator.SetTrigger(ParameterNames.Destroy);
            Destroy(rigidBody);
        }
    }

    private void SetVariables(Rigidbody2D shot, int damage, float shakeDuration, float shakeIntensity)
    {
        if (shot.gameObject == gameObject)
        {
            this.damage = damage;
            this.shakeDuration = shakeDuration;
            this.shakeIntensity = shakeIntensity;
        }
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
