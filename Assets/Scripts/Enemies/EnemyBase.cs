using System;
using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] protected Animator animator;
    [SerializeField] private float hitpoints;
    [SerializeField] private float speed;
    [SerializeField] protected int damage;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int scoreValue;
    [SerializeField] protected float arrowMultiplier;
    [SerializeField] protected float fireMultiplier;
    [SerializeField] protected float thunderMultiplier;
    [SerializeField] protected float earthMultiplier;
    private Collider2D hitbox;
    private GameObject target;
    private WaitForSeconds attackCooldownWFS;
    private bool canAttack = true;
    private bool canMove = false;

    public static Action<GameObject, int> OnDamageDealt;
    public static Action<int> OnEnemyDeath;

    private void Awake()
    {
        attackCooldownWFS = new WaitForSeconds(attackCooldown);
        hitbox = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        ProjectileManager.OnEnemyHit += TakeDamage;
    }

    private void OnDisable()
    {
        ProjectileManager.OnEnemyHit -= TakeDamage;
    }

    private void FixedUpdate()
    {
        if (canMove)
            WalkForwards();

        CalculateDistanceToTarget();
    }

    private void CalculateDistanceToTarget()
    {
        Vector2 origin = (Vector2)transform.position - new Vector2(hitbox.bounds.extents.x, 0f);
        int layerIndex = LayerMask.GetMask("Ally", "Environment");
        RaycastHit2D hitData = Physics2D.Raycast(origin, transform.right * -1, 0.25f, layerIndex);

        if (hitData.collider != null)
        {
            animator.SetBool("IsColliding", true);
            target = hitData.collider.gameObject;
        }
        else
        {
            animator.SetBool("IsColliding", false);
            target = null;
        }
    }

    protected virtual float MultiplyDamage(DamageTypes damageType, float damageReceived)
    {
        switch (damageType)
        {
            case DamageTypes.Pierce:
                damageReceived *= arrowMultiplier;
                break;

            case DamageTypes.Fire:
                damageReceived *= fireMultiplier;
                break;

            case DamageTypes.Thunder:
                damageReceived *= thunderMultiplier;
                break;

            case DamageTypes.Earth:
                damageReceived *= earthMultiplier;
                break;

            default:
                break;
        }

        return damageReceived;
    }

    protected virtual void TakeDamage(GameObject target, DamageTypes damageType, float damageReceived)
    {
        damageReceived = MultiplyDamage(damageType, damageReceived);

        if (target == gameObject)
            hitpoints -= damageReceived;

        if (hitpoints <= 0)
        {
            rigidBody.gravityScale = 0f;
            canMove = false;
            animator.SetTrigger("OnHpEmpty");
        }
    }

    private void StartMove()
    {
        canMove = true;
    }

    private void WalkForwards()
    {
        animator.ResetTrigger("OnAttackCldwn");

        rigidBody.position -= 0.5f * speed * Time.fixedDeltaTime * Vector2.right;
    }

    protected virtual void Attack()
    {
        OnDamageDealt(target, damage);
    }

    protected IEnumerator AttackCooldown()
    {
        if (canAttack)
        {
            canMove = false;
            canAttack = false;
            yield return attackCooldownWFS;

            animator.SetTrigger("OnAttackCldwn");
            canAttack = true;
        }
    }

    private void Die()
    {
        OnEnemyDeath(scoreValue);
        Destroy(gameObject);
    }
}
