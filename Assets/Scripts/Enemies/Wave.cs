using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Enemy Wave")]
public class Wave : ScriptableObject
{
    public EnemyBase enemy;
    public int amount;
    public float interval;
    public float nextWaveDelay;

    [HideInInspector]
    public bool IsDelayOnly => enemy == null;
}
