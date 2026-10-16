using UnityEngine;

public interface IDamageableEnemy
{
    void TakeDamage(GameObject power, PowerTypes powerType, DamageTypes damageType, float damageReceived);
}