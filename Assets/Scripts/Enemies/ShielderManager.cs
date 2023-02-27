using UnityEngine;

public class ShielderManager : EnemyBase
{
    [SerializeField] private float arrowResist;

    protected override void TakeDamage(GameObject target, string damageType, float damageReceived)
    {
        float rand = Random.value;

        if (rand > arrowResist || damageType != "Pierce")
            base.TakeDamage(target, damageType, damageReceived);
    }
}
