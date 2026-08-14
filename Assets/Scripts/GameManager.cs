using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelData currentLevel;
    [SerializeField] private SaveSystem saveSystem;
    [SerializeField] private Animator animator;
    [SerializeField] private MonoBehaviour[] disabledButtons;
    [SerializeField] private int hpBonusScoreMultiplier;
    private readonly HashSet<EnemyBase> enemiesAlive = new();
    private bool hasSpawningFinished = false;
    private int killScore = 0;
    private int starReward = 0;
    private int hpScore;
    private int streakScore;
    private int totalScore;

    public static Action<int> OnScoreChanged;
    public static Action<int, int, int> OnScoreCalculated;
    public static Action<MatchResult> OnResultCalculated;

    private void Start()
    {
        SettingsManager.UpdateSettings(saveSystem.LoadSettings());
    }

    private void OnEnable()
    {
        Time.timeScale = 1.0f;
        HealthManager.OnZeroHealth += FailGame;
        WaveSpawner.OnEnemySpawn += RegisterEnemy;
        WaveSpawner.OnFinishedSpawning += SpawnFinished;
        EnemyBase.OnEnemyDeath += UnregisterEnemy;
    }

    private void OnDisable()
    {
        HealthManager.OnZeroHealth -= FailGame;
        WaveSpawner.OnEnemySpawn -= RegisterEnemy;
        WaveSpawner.OnFinishedSpawning -= SpawnFinished;
        EnemyBase.OnEnemyDeath -= UnregisterEnemy;
    }

    private void EnableAction()
    {
        foreach(MonoBehaviour script in disabledButtons)
            script.enabled = true;
    }

    private void WinGame()
    {
        animator.SetTrigger(ParameterNames.GameIsWon);
        Time.timeScale = 0.3f;
        CalculateScore();
        CalculateResult(true);
    }

    private void CallVictoryLoad()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneNames.LevelSelector);
    }

    private void FailGame()
    {
        animator.SetTrigger(ParameterNames.GameIsOver);
        Time.timeScale = 0.3f;
        CalculateScore();
        CalculateResult(false);
    }

    private void CallGameOver()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneNames.GameOver);
    }

    private void RegisterEnemy(EnemyBase enemy)
    {
        enemiesAlive.Add(enemy);
    }

    private void UnregisterEnemy(int score, EnemyBase enemy)
    {
        enemiesAlive.Remove(enemy);

        IncreaseScore(score);

        if (hasSpawningFinished && enemiesAlive.Count == 0) WinGame();
    }

    private void IncreaseScore(int score)
    {
        killScore += score;
        OnScoreChanged(killScore);
    }

    private void SpawnFinished()
    {
        hasSpawningFinished = true;

        if (enemiesAlive.Count == 0) WinGame();
    }

    private void CalculateScore()
    {
        hpScore = HealthManager.Instance.CurrentHealth * hpBonusScoreMultiplier;
        streakScore = 300;
        totalScore = killScore + hpScore + streakScore;

        OnScoreCalculated(killScore, hpScore, streakScore);
    }

    private void CalculateResult(bool victory)
    {
        if (HealthManager.Instance.CurrentHealth >= HealthManager.Instance.MaxHealth)
            starReward = 3;

        else if (HealthManager.Instance.CurrentHealth >= HealthManager.Instance.MaxHealth * 0.6)
            starReward = 2;

        else if (HealthManager.Instance.CurrentHealth >= HealthManager.Instance.MaxHealth * 0.3)
            starReward = 1;

        MatchResult result = new()
        {
            level = currentLevel,
            victory = victory,
            stars = starReward,
            coinsEarned = ProgressManager.Instance.CurrentProgress.coins + totalScore
        };

        OnResultCalculated(result);
    }
}