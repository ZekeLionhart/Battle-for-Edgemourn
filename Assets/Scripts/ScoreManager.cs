using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreLabel;

    private void Awake()
    {
        UpdateScore(0);
    }

    private void OnEnable()
    {
        GameManager.OnScoreChanged += UpdateScore;
    }
    private void OnDisable()
    {
        GameManager.OnScoreChanged -= UpdateScore;
    }

    private void UpdateScore(int newScore)
    {
        scoreLabel.text = newScore.ToString();
    }
}
