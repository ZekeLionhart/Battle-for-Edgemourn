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
    private readonly HashSet<EnemyBase> enemiesAlive = new();
    private bool hasSpawningFinished = false;
    private int starReward = 0;

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

        if (hasSpawningFinished && enemiesAlive.Count == 0) WinGame();
    }

    private void SpawnFinished()
    {
        hasSpawningFinished = true;

        if (enemiesAlive.Count == 0) WinGame();
    }

    private void CalculateResult(bool victory)
    {
        if (HealthManager.Instance.CurrentHealth == HealthManager.Instance.MaxHealth)
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
            coinsEarned = 0
        };

        OnResultCalculated(result);
    }
}