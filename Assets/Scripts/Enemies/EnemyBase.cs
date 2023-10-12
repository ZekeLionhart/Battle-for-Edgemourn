using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] protected GameObject mainBone;
    [SerializeField] private Collider2D hitbox;
    [SerializeField] protected Animator animator;
    [SerializeField] private AudioSource onAttackSfx;
    [SerializeField] private AudioSource onHitSfx;
    [SerializeField] private AudioSource gruntSfx;
    [SerializeField] private AudioSource onDeathSfx;
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
    private GameObject previousHit;
    private GameObject target;
    private WaitForSeconds attackCooldownWFS;
    private List<string> targetStrings;
    private bool canAttack = true;
    private bool canMove = false;
    private bool isDead = false;

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
        ArrowManager.OnEnemyHitWithArrow += PinArrow;
    }

    private void OnDisable()
    {
        ProjectileManager.OnEnemyHit -= TakeDamage;
        ArrowManager.OnEnemyHitWithArrow -= PinArrow;
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
            animator.SetBool(ParameterNames.IsColliding, true);
            target = hitData.collider.gameObject;
        }
        else
        {
            animator.SetBool(ParameterNames.IsColliding, false);
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

    protected void TakeDamage(GameObject power, GameObject target, PowerTypes powerType, DamageTypes damageType, float damageReceived)
    {
        if (target == gameObject && power != previousHit)
        {
            previousHit = power;
            damageReceived = MultiplyDamage(damageType, damageReceived);
            if (damageType == DamageTypes.Pierce)
                onHitSfx.Play();
            hitpoints -= damageReceived;
            CallDamageAnalytics(powerType, damageReceived);
        }

        if (hitpoints <= 0 && !isDead)
        {
            isDead = true;
            Destroy(rigidBody);
            animator.SetTrigger(ParameterNames.OnHpEmpty);
            onDeathSfx.Play();
            CallKillAnalytics(powerType, scoreValue);
        }
        else if (gruntSfx != null)
            gruntSfx.Play();
    }

    protected virtual void PinArrow(GameObject power, GameObject target, Rigidbody2D arrow, PowerTypes powerType, DamageTypes damageType, float damageReceived)
    {
        if (target == gameObject)
            arrow.transform.parent = mainBone.transform;

        TakeDamage(power, target, powerType, damageType, damageReceived);
    }

    private void StartMove()
    {
        canMove = true;
    }

    private void WalkForwards()
    {
        animator.ResetTrigger(ParameterNames.OnAttackCldwn);

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

            animator.SetTrigger(ParameterNames.OnAttackCldwn);
            canAttack = true;
        }
    }

    private void Die()
    {
        OnEnemyDeath(scoreValue);
        Destroy(gameObject);
    }

    private void PlayAttackSfx()
    {
        if (onAttackSfx != null)
            onAttackSfx.Play();
    }

    protected virtual void CallDamageAnalytics(PowerTypes power, float damage)
    {
        if (hitpoints < 0)
            damage += hitpoints;

        Analytics.OnPowerDamage(power, damage);
    }

    protected virtual void CallKillAnalytics(PowerTypes power, float credits)
    {
        Analytics.OnPowerKill(power, credits);
    }
}
