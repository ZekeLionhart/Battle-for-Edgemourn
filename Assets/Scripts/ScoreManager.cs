using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreLabel;
    private int score;

    private void Awake()
    {
        score = 0;
        UpdateScore();
    }

    private void OnEnable()
    {
        EnemyBase.OnEnemyDeath += AddScore;
    }
    private void OnDisable()
    {
        EnemyBase.OnEnemyDeath += AddScore;
    }

    private void AddScore(int points)
    {
        score += points;
        UpdateScore();
    }

    private void UpdateScore()
    {
        scoreLabel.text = score.ToString();
    }
}
