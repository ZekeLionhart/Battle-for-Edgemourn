using UnityEngine;

public class ShielderManager : EnemyBase
{
    [Header("--Individual Fields--")]
    [SerializeField] private AudioSource onDefendSfx;
    [SerializeField] private Rigidbody2D deadArrow;

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
            Rigidbody2D newArrow = Instantiate(deadArrow, transform.position + Vector3.up / 2, rotation, null);
            Destroy(arrow.gameObject);
            newArrow.velocity = new Vector2(2f, 3f);
            newArrow.angularVelocity = -360f * (rand * 5);

            return false;
        }
    }
}
