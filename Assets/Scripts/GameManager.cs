using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelData currentLevel;
    [SerializeField] private SaveSystem saveSystem;
    [SerializeField] private AudioSource bgm;
    [SerializeField] private AudioSource victorySFX;
    [SerializeField] private AudioSource defeatSFX;
    [SerializeField] private MonoBehaviour[] disabledButtons;
    [SerializeField] private float earlyScoreMultiplier;
    [SerializeField] private float streakScoreMultiplier;
    [SerializeField] private int hpScoreMultiplier;
    private readonly HashSet<EnemyBase> enemiesAlive = new();
    private bool hasSpawningFinished = false;
    private int killScore = 0;
    private int earlyScore = 0;
    private int hpScore;
    private int streakScore;
    private int totalScore;

    public static Action<int> OnScoreChanged;
    public static Action<int, Transform> OnScoreEarned;
    public static Action<bool, bool, bool, int, int, int, int> OnScoreCalculated;
    public static Action<MatchResult> OnResultCalculated;
    public static Action OnGameEnded;

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
        ResultViewer.CallMusicStop += StopMusic;
    }

    private void OnDisable()
    {
        HealthManager.OnZeroHealth -= FailGame;
        WaveSpawner.OnEnemySpawn -= RegisterEnemy;
        WaveSpawner.OnFinishedSpawning -= SpawnFinished;
        EnemyBase.OnEnemyDeath -= UnregisterEnemy;
        ResultViewer.CallMusicStop -= StopMusic;
    }

    private void EnableAction()
    {
        foreach (MonoBehaviour script in disabledButtons)
            script.enabled = true;
    }

    private void DisableAction()
    {
        foreach (MonoBehaviour script in disabledButtons)
            script.enabled = false;
    }

    private void WinGame()
    {
        victorySFX.Play();
        Time.timeScale = 0.3f;
        DisableAction();
        CalculateResult(true);
    }

    private void FailGame()
    {
        defeatSFX.Play();
        Time.timeScale = 0.3f;
        OnGameEnded();
        DisableAction();
        CalculateResult(false);
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

    private void CalculateResult(bool isVictory)
    {
        hpScore = HealthManager.Instance.CurrentHealth * hpScoreMultiplier;
        totalScore = killScore + earlyScore + streakScore + hpScore;

        bool metTargetScore = false;
        bool isPerfectDefense = false;

        if (isVictory)
        {
            if (totalScore >= currentLevel.targetScore)
                metTargetScore = true;

            if (HealthManager.Instance.CurrentHealth >= HealthManager.Instance.MaxHealth)
                isPerfectDefense = true;
        }

        OnScoreCalculated(isVictory, metTargetScore, isPerfectDefense, killScore, earlyScore, streakScore, hpScore);

        MatchResult result = new()
        {
            level = currentLevel,
            victory = isVictory,
            targetScore = metTargetScore,
            perfectDefense = isPerfectDefense,
            coinsEarned = totalScore
        };

        OnResultCalculated(result);
    }

    private void StopMusic()
    {
        bgm.enabled = false;
    }
}