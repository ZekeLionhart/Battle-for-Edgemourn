using System.Collections;
using TMPro;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI killScore;
    [SerializeField] private TextMeshProUGUI hPScore;
    [SerializeField] private TextMeshProUGUI streakScore;
    [SerializeField] private TextMeshProUGUI totalScore;
    [SerializeField] private TextMeshProUGUI totalScoreToMove;
    [SerializeField] private TextMeshProUGUI moraleScore;
    private int maxKillScore;
    private int maxHPScore;
    private int maxStreakScore;
    private int maxTotalScore;
    private int stashedMoraleScore;

    private void OnEnable()
    {
        GameManager.OnScoreCalculated += SetScores;
    }

    private void OnDisable()
    { 
        GameManager.OnScoreCalculated -= SetScores;
    }

    private void Start()
    {
        moraleScore.text = (stashedMoraleScore = ProgressManager.Instance.CurrentProgress.coins).ToString();
    }

    private void SetScores(int newKillScore, int newHPScore, int newStreakScore)
    {
        maxKillScore = newKillScore;
        maxHPScore = newHPScore;
        maxStreakScore = newStreakScore;
        maxTotalScore = maxKillScore + maxHPScore + maxStreakScore;
    }

    public void RunKillScore()
    {
        StartCoroutine(RunScoreCount(killScore, 0, maxKillScore));
    }

    public void RunHPScore()
    {
        StartCoroutine(RunScoreCount(hPScore, 0, maxHPScore));
    }

    public void RunStreakScore()
    {
        StartCoroutine(RunScoreCount(streakScore, 0, maxStreakScore));
    }

    public void RunFinalScore()
    {
        StartCoroutine(RunScoreCount(totalScore, 0, maxTotalScore));
        totalScoreToMove.text = maxTotalScore.ToString();
    }

    public void RunAddToMorale()
    {
        StartCoroutine(RunScoreCount(moraleScore, stashedMoraleScore, maxTotalScore));

    }

    private IEnumerator RunScoreCount(TextMeshProUGUI text, int startScore, int maxScore)
    {
        float elapsed = 0f;

        while (elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;

            float progress = elapsed / 0.55f;
            int currentScore = Mathf.RoundToInt(Mathf.Lerp(startScore, startScore + maxScore, progress));

            text.text = currentScore.ToString();

            yield return null;
        }

        text.text = (startScore + maxScore).ToString();
    }
}
