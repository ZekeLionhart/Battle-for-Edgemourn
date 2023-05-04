using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private Collider2D hitbox;
    [SerializeField] protected Animator animator;
    [SerializeField] protected TargetTypes[] targets;
    [SerializeField] private float hitpoints;
    [SerializeField] private float speed;
    [SerializeField] protected int damage;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackRange;
    [SerializeField] private int scoreValue;
    [SerializeField] protected float arrowMultiplier;
    [SerializeField] protected float fireMultiplier;
    [SerializeField] protected float thunderMultiplier;
    [SerializeField] protected float earthMultiplier;
    private GameObject target;
    private WaitForSeconds attackCooldownWFS;
    private List<string> targetStrings;
    private bool canAttack = true;
    private bool canMove = false;

    public static Action<GameObject, int> OnDamageDealt;
    public static Action<int> OnEnemyDeath;

    private void Awake()
    {
        attackCooldownWFS = new WaitForSeconds(attackCooldown);
        targetStrings = new List<string>();

        foreach (TargetTypes targetEnum in targets)
            targetStrings.Add(Enum.GetName(typeof(TargetTypes), targetEnum));
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

    protected virtual void CalculateDistanceToTarget()
    {
        Vector2 origin = (Vector2)transform.position - new Vector2(hitbox.bounds.extents.x, -0.5f);
        int layerIndex = LayerMask.GetMask(targetStrings.ToArray());
        RaycastHit2D hitData = Physics2D.Raycast(origin, transform.right * -1, attackRange, layerIndex);
        Debug.DrawRay(origin, transform.right * -attackRange);

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
            Destroy(rigidBody);
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

        if (rigidBody != null)
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
