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

    protected override void PinArrow(GameObject power, GameObject target, Rigidbody2D arrow, PowerTypes powerType, DamageTypes damageType, float damageReceived)
    {
        if (target == gameObject)
        {
            float rand = Random.value;

            if (rand > arrowMultiplier)
            {
                arrow.transform.parent = mainBone.transform;
                arrow.simulated = false;
                TakeDamage(power, target, powerType, damageType, damageReceived);
            }
            else
            {
                onDefendSfx.Play();
                Rigidbody2D newArrow = Instantiate(deadArrow, transform.position + Vector3.up / 2, Quaternion.identity, null);
                Destroy(arrow.gameObject);
                newArrow.velocity = new Vector2(2f, 3f);
                newArrow.angularVelocity = -360f * (rand * 5);
            }
        }
    }
}
