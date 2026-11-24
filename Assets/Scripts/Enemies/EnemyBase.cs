using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBase : PoolableObject, IDamageableEnemy, IArrowHitReceiver
{
    [Header("------References------")]
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] protected GameObject mainBone;
    [SerializeField] private Collider2D hitbox;
    [SerializeField] protected Animator animator;

    [Header("---------VFX---------")]
    [SerializeField] protected float shakeDuration;
    [SerializeField] protected float shakeIntensity;
    [SerializeField] private Transform scoreVFXOrigin;
    [SerializeField] protected GameObject gore;
    [SerializeField] protected ParticleSystem[] dustParticles;

    [Header("---------SFX---------")]
    [SerializeField] private AudioSource[] onMoveSfx;
    [SerializeField] private AudioSource onAttackSfx;
    [SerializeField] private AudioSource onHitSfx;
    [SerializeField] private AudioSource gruntSfx;
    [SerializeField] private AudioSource onDeathSfx;

    [Header("--------Stats--------")]
    [SerializeField] protected TargetTypes[] targets;
    [SerializeField] private float hitpoints;
    [SerializeField] private float speed;
    [SerializeField] protected int damage;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackRange;
    [SerializeField] private int scoreValue;

    [Header("-----Resistances-----")]
    [SerializeField] protected float arrowMultiplier;
    [SerializeField] protected float fireMultiplier;
    [SerializeField] protected float thunderMultiplier;
    [SerializeField] protected float earthMultiplier;

    private PoolableObject previousHit;
    private ulong previousHitUseId;
    private GameObject target;
    private WaitForSeconds attackCooldownWFS;
    private List<string> targetStrings;
    private float currentHitpoints;
    private bool canAttack;
    private bool canMove;
    protected bool isDead;
    protected bool hasGameEnded;
    private float spawnTime;
    protected float bonusWindow = 15f;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private int savedDamage;
    private bool noDamageEnabled;
#endif

    public float Lifetime => Time.time - spawnTime;
    public int CoinsReward => scoreValue;

    public static event Action<int, bool, EnemyBase, Transform> OnEnemyDeath;
    public event Action OnReturnedToPool;

    protected virtual void Awake()
    {
        attackCooldownWFS = new WaitForSeconds(attackCooldown);
        targetStrings = new List<string>();

        animator.writeDefaultValuesOnDisable = true;

        foreach (TargetTypes targetEnum in targets)
            targetStrings.Add(Enum.GetName(typeof(TargetTypes), targetEnum));
    }

    private void OnEnable()
    {
        GameManager.OnGameEnded += EndGame;
    }

    private void OnDisable()
    {
        GameManager.OnGameEnded -= EndGame;
    }

    private void FixedUpdate()
    {
        if (isDead) return;

        if (canMove)
            WalkForwards();

        CalculateDistanceToTarget();
    }

    public void SpawnEnemy()
    {
        currentHitpoints = hitpoints;
        canAttack = true;
        canMove = false;
        isDead = false;
        hasGameEnded = false;
        target = null;
        previousHit = null;
        spawnTime = Time.time;

        gameObject.SetActive(true);
        animator.Update(0f);

        rigidBody.simulated = true;
        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.angularVelocity = 0;
    }

    private bool VerifyEarlyKill()
    {
        if (Lifetime <= bonusWindow / speed) //slower enemies retain the bonus for longer
            return true;
        else
            return false;
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

    public void TakeDamage(PoolableObject power, PowerTypes powerType, DamageTypes damageType, float damageReceived)
    {
        if (power == previousHit && power.UseId == previousHitUseId)
            return;

        previousHit = power;
        previousHitUseId = power.UseId;
        damageReceived = MultiplyDamage(damageType, damageReceived);
        onHitSfx.pitch = Random.Range(0.9f, 1.1f);
        if (damageType == DamageTypes.Pierce)
            onHitSfx.Play();
        currentHitpoints -= damageReceived;
        CallDamageAnalytics(powerType, damageReceived);

        if (currentHitpoints <= 0 && !isDead)
        {
            VerifyEarlyKill();

            InitiateDeath();
            CallKillAnalytics(powerType, scoreValue);
        }
        else if (gruntSfx != null && !isDead)
        {
            gruntSfx.pitch = Random.Range(0.9f, 1.1f);
            gruntSfx.Play();
        }
    }

    public virtual bool PinArrow(Vector2 hitPoint, Quaternion rotation, Rigidbody2D arrow)
    {
        arrow.transform.parent = mainBone.transform;

        if (isDead) return false;

        SprayGore(rotation, hitPoint);

        return true;
    }

    protected virtual void SprayGore(Quaternion rotation, Vector2 hitPoint)
    {
        if (gore != null) Instantiate(gore, hitPoint, rotation);
    }

    private void StartMove()
    {
        canMove = true;
    }

    private void CallMovementSFX()
    {
        foreach (AudioSource sfx in onMoveSfx)
        {
            sfx.pitch = Random.Range(0.9f, 1.1f);
            sfx.Play();
        }
    }

    private void StopMovementSFX()
    {
        foreach (AudioSource sfx in onMoveSfx)
            sfx.Stop();
    }

    private void WalkForwards()
    {
        animator.ResetTrigger(ParameterNames.OnAttackCldwn);

        if (rigidBody != null)
            rigidBody.position -= 0.5f * speed * Time.fixedDeltaTime * Vector2.right;
    }

    protected virtual void Attack()
    {
        if (target == null || hasGameEnded) return;

        if (target.TryGetComponent<IDamageable>(out IDamageable receiver))
            receiver.TakeDamage(damage);

        CameraShake.Instance.Shake(shakeDuration, shakeIntensity);
    }

    protected IEnumerator AttackCooldown()
    {
        canMove = false;

        if (canAttack)
        {
            canAttack = false;
            yield return attackCooldownWFS;

            if (!hasGameEnded) animator.SetTrigger(ParameterNames.OnAttackCldwn);
            canAttack = true;
        }
    }

    private void InitiateDeath()
    {
        StopAllCoroutines();
        canMove = false;
        isDead = true;
        rigidBody.simulated = false;
        animator.SetTrigger(ParameterNames.OnHpEmpty);
        onDeathSfx.pitch = Random.Range(0.9f, 1.1f);
        onDeathSfx.Play();
        OnEnemyDeath(scoreValue, VerifyEarlyKill(), this, scoreVFXOrigin);
    }

    protected virtual void ReturnToPool()
    {
        OnReturnedToPool?.Invoke();

        foreach (ParticleSystem particle in dustParticles)
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        if (onMoveSfx != null)
            foreach (AudioSource sfx in onMoveSfx)
                sfx.Stop();
        if (onAttackSfx)
            onAttackSfx.Stop();
        if (onHitSfx)
            onHitSfx.Stop();
        if (gruntSfx)
            gruntSfx.Stop();
        if (onDeathSfx)
            onDeathSfx.Stop();

        ReleaseToPool();
    }

    private void EndGame()
    {
        hasGameEnded = true;
    }

    private void PlayAttackSfx()
    {
        if (onAttackSfx == null) return;

        onAttackSfx.pitch = Random.Range(0.9f, 1.1f);
        onAttackSfx.Play();
    }

    protected virtual void CallDamageAnalytics(PowerTypes power, float damage)
    {
        if (currentHitpoints < 0)
            damage += currentHitpoints;

        Analytics.OnPowerDamage(power, damage);
    }

    protected virtual void CallKillAnalytics(PowerTypes power, float credits)
    {
        Analytics.OnPowerKill(power, credits);
    }

    public void SummonDustPasticles()
    {
        foreach (ParticleSystem particles in dustParticles)
            particles.Play();
    }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    public void DebugDeath()
    {
        if (isDead) return;

        InitiateDeath();
    }

    public void DebugEnableNoDamage()
    {
        if (isDead) return;

        if (!noDamageEnabled)
        {
            savedDamage = damage;
            noDamageEnabled = true;
        }

        damage = 0;
    }

    public void DebugDisableNoDamage()
    {
        if (!noDamageEnabled) return;

        damage = savedDamage;
        noDamageEnabled = false;
    }
#endif
}
