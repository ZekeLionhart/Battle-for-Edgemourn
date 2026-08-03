using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Wave")]
public class Wave : ScriptableObject
{
    public EnemyBase enemy;
    public int amount;
    public float interval;
    public float startDelay;
}
