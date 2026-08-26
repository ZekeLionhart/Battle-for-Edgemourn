using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultViewer : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Image[] victorySideDecorations;
    [SerializeField] private Image[] defeatSideDecorations;
    [SerializeField] private Image victoryBackground;
    [SerializeField] private Image defeatBackground;
    [SerializeField] private Animator victoryStar;
    [SerializeField] private Animator scoreStar;
    [SerializeField] private Animator defenseStar;
    [SerializeField] private Image banner;
    [SerializeField] private Color bannerVictoryColor;
    [SerializeField] private Color bannerDefeatColor;
    [SerializeField] private AudioSource victoryBGM;
    [SerializeField] private AudioSource defeatBGM;
    [SerializeField] private AudioSource scoreSFX;
    [SerializeField] private AudioSource starHitSFX;
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

    private void SetUpResultScreen(bool isVictory, bool metTargetScore, bool isPerfectDefense, 
        int newKillScore, int newEarlyScore, int newStreakScore, int newHPScore)
    {
        if (isVictory)
        {
            title.text = TextDB.Victory;
            victoryStar.gameObject.SetActive(true);
        }
        else
            title.text = TextDB.Defeat;

        if (metTargetScore)
            scoreStar.gameObject.SetActive(true);

        if (isPerfectDefense)
            defenseStar.gameObject.SetActive(true);

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
            foreach (Image deco in defeatSideDecorations)
                deco.gameObject.SetActive(false);
            victoryBackground.gameObject.SetActive(true);
            banner.color = bannerVictoryColor;
            victoryBGM.Play();
        }
        else
        {
            foreach (Image deco in victorySideDecorations)
                deco.gameObject.SetActive(false);
            defeatBackground.gameObject.SetActive(true);
            banner.color = bannerDefeatColor;
            defeatBGM.Play();
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
        int counterSFX = 0;
        float elapsed = 0f;

        while (elapsed < 0.5f)
        {
            elapsed += Time.deltaTime;

            float progress = elapsed / 0.55f;
            int currentScore = Mathf.RoundToInt(Mathf.Lerp(startScore, startScore + maxScore, progress));

            text.text = currentScore.ToString();

            if (maxScore > 0)
            {
                if (counterSFX == 0)
                {
                    scoreSFX.Play();
                    counterSFX++;
                }
                else if (counterSFX < 20)
                    counterSFX++;
                else
                    counterSFX = 0;
            }

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

    private void SpawnVictoryStar()
    {
        if (victoryStar.gameObject.activeInHierarchy)
            victoryStar.SetTrigger(ParameterNames.Spawn);
    }

    private void SpawnScoreStar()
    {
        if (scoreStar.gameObject.activeInHierarchy)
            scoreStar.SetTrigger(ParameterNames.Spawn);
    }

    private void SpawnDefenseStar()
    {
        if (defenseStar.gameObject.activeInHierarchy)
            defenseStar.SetTrigger(ParameterNames.Spawn);
    }

    private void ResumeTime()
    {
        Time.timeScale = 1f;
    }

    private void Restart()
    {
        PlayerPrefs.SetString("Restart", SceneManager.GetActiveScene().name);

#if UNITY_ANDROID
        LevelPlayAdsManager.CallInterstitial(true);
#else
        SceneManager.LoadScene(SceneNames.RestartTransition);
#endif
    }

    private void QuitLevel()
    {
#if UNITY_ANDROID
        LevelPlayAdsManager.CallInterstitial(false);
#else
        SceneManager.LoadScene(SceneNames.LevelSelector);
#endif
    }
}
