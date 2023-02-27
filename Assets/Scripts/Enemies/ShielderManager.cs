using UnityEngine;

public class ShielderManager : EnemyBase
{
    [SerializeField] private float arrowResist;

    protected override void TakeDamage(GameObject target, DamageTypes damageType, float damageReceived)
    {
        float rand = Random.value;

        if (rand > arrowResist || damageType != DamageTypes.Pierce)
            base.TakeDamage(target, damageType, damageReceived);
    }
}
