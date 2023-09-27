using UnityEngine;

public class BoulderManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidBody;
    private int damage;

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

    private void SetVariables(Rigidbody2D shot, int damage)
    {
        if (shot.gameObject == gameObject)
            this.damage = damage;
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
