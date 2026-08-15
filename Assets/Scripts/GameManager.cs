using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelData currentLevel;
    [SerializeField] private SaveSystem saveSystem;
    [SerializeField] private Animator animator;
    [SerializeField] private MonoBehaviour[] disabledButtons;
    [SerializeField] private float earlyScoreMultiplier;
    [SerializeField] private float streakScoreMultiplier;
    [SerializeField] private int hpScoreMultiplier;
    private readonly HashSet<EnemyBase> enemiesAlive = new();
    private bool hasSpawningFinished = false;
    private int starReward = 0;
    private int killScore = 0;
    private int earlyScore = 0;
    private int hpScore;
    private int streakScore;
    private int totalScore;

    public static Action<int> OnScoreChanged;
    public static Action<int, Transform> OnScoreEarned;
    public static Action<int, int, int, int> OnScoreCalculated;
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

    private void UnregisterEnemy(int newKillScore, bool isEarlyKill, EnemyBase enemy, Transform popupCoord)
    {
        int newEarlyBonus = 0;
        int currentStreak = StreakManager.Instance.RegisterKill(enemy, popupCoord);

        if (isEarlyKill)
            newEarlyBonus = (int)(newKillScore * earlyScoreMultiplier);

        int newStreakScore = (currentStreak - 1) * (int)(streakScoreMultiplier * (newKillScore + newEarlyBonus));

        OnScoreEarned(newKillScore + newEarlyBonus + newStreakScore, popupCoord);
        IncreaseScore(newKillScore, newEarlyBonus, newStreakScore);

        enemiesAlive.Remove(enemy);

        if (hasSpawningFinished && enemiesAlive.Count == 0) WinGame();
    }

    private void IncreaseScore(int newkillScore, int newEarlyBonus, int newStreakBonus)
    {
        killScore += newkillScore;
        earlyScore += newEarlyBonus;
        streakScore += newStreakBonus;
        OnScoreChanged(killScore + earlyScore + streakScore);
    }

    private void SpawnFinished()
    {
        hasSpawningFinished = true;

        if (enemiesAlive.Count == 0) WinGame();
    }

    private void CalculateScore()
    {
        hpScore = HealthManager.Instance.CurrentHealth * hpScoreMultiplier;
        totalScore = killScore + earlyScore + streakScore + hpScore;

        OnScoreCalculated(killScore, earlyScore, streakScore, hpScore);
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
            coinsEarned = totalScore
        };

        OnResultCalculated(result);
    }
}