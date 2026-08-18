using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultViewer : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Image victoryBackground;
    [SerializeField] private Image defeatBackground;
    [SerializeField] private AudioSource victoryBGM;
    [SerializeField] private AudioSource defeatBGM;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI killScore;
    [SerializeField] private TextMeshProUGUI earlyScore;
    [SerializeField] private TextMeshProUGUI streakScore;
    [SerializeField] private TextMeshProUGUI hPScore;
    [SerializeField] private TextMeshProUGUI totalScore;
    [SerializeField] private TextMeshProUGUI totalScoreToMove;
    [SerializeField] private TextMeshProUGUI moraleScore;
    [SerializeField] private TextMeshProUGUI restartButton;
    [SerializeField] private TextMeshProUGUI quitButton;
    private int maxKillScore;
    private int maxEarlyScore;
    private int maxStreakScore;
    private int maxHPScore;
    private int maxTotalScore;
    private int stashedMoraleScore;
    private bool isVictory;

    public static Action CallMusicStop;

    private void OnEnable()
    {
        GameManager.OnScoreCalculated += SetUpResultScreen;
    }

    private void OnDisable()
    { 
        GameManager.OnScoreCalculated -= SetUpResultScreen;
    }

    private void Start()
    {
        moraleScore.text = (stashedMoraleScore = ProgressManager.Instance.CurrentProgress.coins).ToString();
    }

    private void SetUpResultScreen(bool isVictory, int newKillScore, int newEarlyScore, int newStreakScore, int newHPScore)
    {
        if (isVictory)
            title.text = TextDB.Victory;
        else
            title.text = TextDB.Defeat;

        this.isVictory = isVictory;
        maxKillScore = newKillScore;
        maxEarlyScore = newEarlyScore;
        maxStreakScore = newStreakScore;
        maxHPScore = newHPScore;
        maxTotalScore = maxKillScore + maxEarlyScore + maxStreakScore + maxHPScore;

        CallMusicStop();
        animator.SetTrigger(ParameterNames.MatchEnded);
    }

    public void ChooseTheme()
    {
        if (isVictory)
        {
            victoryBackground.gameObject.SetActive(true);
            victoryBGM.gameObject.SetActive(true);
        }
        else
        {
            defeatBackground.gameObject.SetActive(true);
            defeatBGM.gameObject.SetActive(true);
        }
    }

    public void RunKillScore()
    {
        StartCoroutine(RunScoreCount(killScore, 0, maxKillScore));
    }

    public void RunEarlyScore()
    {
        StartCoroutine(RunScoreCount(earlyScore, 0, maxEarlyScore));
    }

    public void RunStreakScore()
    {
        StartCoroutine(RunScoreCount(streakScore, 0, maxStreakScore));
    }

    public void RunHPScore()
    {
        StartCoroutine(RunScoreCount(hPScore, 0, maxHPScore));
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

    public void ChooseButtons()
    {
        if (!isVictory)
        {
            restartButton.gameObject.SetActive(true);
            quitButton.text = TextDB.GetTextByKey(UITextKey.Quit);
        }
        else quitButton.text = TextDB.GetTextByKey(UITextKey.Continue);
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

    private void MoveTotalScore()
    {
        StartCoroutine(MoveTotalScoreCoroutine());
    }

    private IEnumerator MoveTotalScoreCoroutine()
    {
        Vector3 startPosition = totalScoreToMove.transform.position;
        Vector3 targetPosition = moraleScore.transform.position
            + new Vector3(totalScoreToMove.rectTransform.rect.width / 2, 0);

        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float time = elapsed / duration;

            totalScoreToMove.transform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                time
            );

            yield return null;
        }

        totalScoreToMove.transform.position = targetPosition;
    }

    private void ResumeTime()
    {
        Time.timeScale = 1f;
    }

    private void Restart()
    {
        PlayerPrefs.SetString("Restart", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneNames.RestartTransition);
    }

    private void QuitLevel()
    {
        SceneManager.LoadScene(SceneNames.LevelSelector);
    }
}
