using UnityEngine;

public class ShielderManager : EnemyBase
{
    protected override float MultiplyDamage(DamageTypes damageType, float damageReceived)
    {
        if (damageType != DamageTypes.Pierce)
            return base.MultiplyDamage(damageType, damageReceived);
        else 
            return damageReceived;
    }

    protected override void TakeDamage(GameObject target, DamageTypes damageType, float damageReceived)
    {
        float rand = Random.value;

        if (rand > arrowMultiplier || damageType != DamageTypes.Pierce)
            base.TakeDamage(target, damageType, damageReceived);
    }
}
