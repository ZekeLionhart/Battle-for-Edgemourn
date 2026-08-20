using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBase : MonoBehaviour
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

    private GameObject previousHit;
    private GameObject target;
    private WaitForSeconds attackCooldownWFS;
    private List<string> targetStrings;
    private bool canAttack = true;
    private bool canMove = false;
    protected bool isDead = false;
    private bool hasGameEnded = false;
    private float spawnTime;
    protected float bonusWindow = 15f;

    public float Lifetime => Time.time - spawnTime;
    public int CoinsReward => scoreValue;

    public static Action<GameObject, int> OnDamageDealt;
    public static Action<int, bool, EnemyBase, Transform> OnEnemyDeath;

    protected virtual void Awake()
    {
        spawnTime = Time.time;
        attackCooldownWFS = new WaitForSeconds(attackCooldown);
        targetStrings = new List<string>();

        foreach (TargetTypes targetEnum in targets)
            targetStrings.Add(Enum.GetName(typeof(TargetTypes), targetEnum));
    }

    private void OnEnable()
    {
        ProjectileManager.OnEnemyHit += TakeDamage;
        ArrowManager.OnEnemyHitWithArrow += PinArrow;
        GameManager.OnGameEnded += EndGame;
    }

    private void OnDisable()
    {
        ProjectileManager.OnEnemyHit -= TakeDamage;
        ArrowManager.OnEnemyHitWithArrow -= PinArrow;
        GameManager.OnGameEnded -= EndGame;
    }

    private void FixedUpdate()
    {
        if (canMove)
            WalkForwards();

        CalculateDistanceToTarget();
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

    protected void TakeDamage(GameObject power, GameObject target, PowerTypes powerType, DamageTypes damageType, float damageReceived)
    {
        if (target != gameObject || power == previousHit) return;

        previousHit = power;
        damageReceived = MultiplyDamage(damageType, damageReceived);
        onHitSfx.pitch = Random.Range(0.9f, 1.1f);
        if (damageType == DamageTypes.Pierce)
            onHitSfx.Play();
        hitpoints -= damageReceived;
        CallDamageAnalytics(powerType, damageReceived);

        if (hitpoints <= 0 && !isDead)
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

    protected virtual void PinArrow(GameObject power, GameObject target, Vector2 hitPoint, Quaternion rotation, Rigidbody2D arrow, PowerTypes powerType, DamageTypes damageType, float damageReceived)
    {
        if (target != gameObject) return;
        
        arrow.transform.parent = mainBone.transform;

        if (isDead) return;

        TakeDamage(power, target, powerType, damageType, damageReceived);
        SprayGore(rotation, hitPoint);
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
        OnDamageDealt(target, damage);
        CameraShake.CallShake(shakeDuration, shakeIntensity);
    }

    protected IEnumerator AttackCooldown()
    {
        if (canAttack)
        {
            canMove = false;
            canAttack = false;
            yield return attackCooldownWFS;

            if (!hasGameEnded) animator.SetTrigger(ParameterNames.OnAttackCldwn);
            canAttack = true;
        }
    }

    private void InitiateDeath()
    {
        isDead = true;
        Destroy(rigidBody);
        animator.SetTrigger(ParameterNames.OnHpEmpty);
        onDeathSfx.pitch = Random.Range(0.9f, 1.1f);
        onDeathSfx.Play();
        OnEnemyDeath(scoreValue, VerifyEarlyKill(), this, scoreVFXOrigin);
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void EndGame()
    {
        hasGameEnded = true;
    }

    private void PlayAttackSfx()
    {
        if (onAttackSfx != null)
        {
            onAttackSfx.pitch = Random.Range(0.9f, 1.1f);
            onAttackSfx.Play();
        }
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

    public void SummonDustPasticles()
    {
        foreach (ParticleSystem particles in dustParticles)
            particles.Play();
    }
}
