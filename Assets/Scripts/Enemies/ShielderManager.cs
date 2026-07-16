using UnityEngine;

public class ShielderManager : EnemyBase
{
    [SerializeField] private AudioSource onDefendSfx;
    [SerializeField] private Rigidbody2D deadArrow;

    protected override float MultiplyDamage(DamageTypes damageType, float damageReceived)
    {
        if (damageType != DamageTypes.Pierce)
            return base.MultiplyDamage(damageType, damageReceived);
        else 
            return damageReceived;
    }

    protected override void PinArrow(GameObject power, GameObject target, Vector2 hitPoint, Quaternion rotation, Rigidbody2D arrow, PowerTypes powerType, DamageTypes damageType, float damageReceived)
    {
        if (target != gameObject) return;
        
        float rand = Random.value;

        if (rand > arrowMultiplier)
        {
            arrow.transform.parent = mainBone.transform;
            arrow.simulated = false;

            if (isDead) return;

            TakeDamage(power, target, powerType, damageType, damageReceived);
            SprayGore(rotation, hitPoint);
        }
        else
        {
            onDefendSfx.Play();
            Rigidbody2D newArrow = Instantiate(deadArrow, transform.position + Vector3.up / 2, rotation, null);
            Destroy(arrow.gameObject);
            newArrow.velocity = new Vector2(2f, 3f);
            newArrow.angularVelocity = -360f * (rand * 5);
        }
    }
}
