using UnityEngine;

public class ShielderManager : EnemyBase
{
    [Header("--Individual Fields--")]
    [SerializeField] private AudioSource onDefendSfx;

    protected override float MultiplyDamage(DamageTypes damageType, float damageReceived)
    {
        if (damageType != DamageTypes.Pierce)
            return base.MultiplyDamage(damageType, damageReceived);
        else 
            return damageReceived;
    }

    public override bool PinArrow(Vector2 hitPoint, Quaternion rotation, Rigidbody2D arrow)
    {
        float rand = Random.value;

        if (rand > arrowMultiplier)
        {
            arrow.transform.parent = mainBone.transform;
            arrow.simulated = false;

            if (isDead) return false;

            SprayGore(rotation, hitPoint);

            return true;
        }
        else
        {
            onDefendSfx.Play();
            ArrowManager deflectedArrow = arrow.GetComponent<ArrowManager>();
            deflectedArrow.Deflect(rand);

            return false;
        }
    }
}
