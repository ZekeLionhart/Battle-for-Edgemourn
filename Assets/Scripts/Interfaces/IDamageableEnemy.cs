using UnityEngine;

public interface IDamageableEnemy
{
    void TakeDamage(PoolableObject power, PowerTypes powerType, DamageTypes damageType, float damageReceived);
}