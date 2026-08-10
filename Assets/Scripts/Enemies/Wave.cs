using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Enemy Wave")]
public class Wave : ScriptableObject
{
    public EnemyBase enemy;
    public int amount;
    public float interval;
    public float nextWaveDelay;

    public bool IsDelayOnly => enemy == null;

    [Header("Balance Data")]
    [SerializeField] private int minCoins;
    [SerializeField] private int maxCoins;

    private void OnValidate()
    {
        if (enemy != null)
        {
            minCoins = CalculateCoins();
            maxCoins = CalculateCoins() * 2;
        }
    }

    private int CalculateCoins()
    {
        return enemy.CoinsReward * amount;
    }
}
