using System;
using UnityEngine;

public class StreakManager : MonoBehaviour
{
    public static StreakManager Instance { get; private set; }

    [SerializeField] private float streakWindow;
    private EnemyBase previousEnemy;
    private int currentStreak;
    private float startTime;
    private float timer;
    private int totalStreakBonus;
    private bool timerIsRunning = false;

    public float StreakWindow => streakWindow;
    public float Timer => timer;

    public static Action<int> OnStreakIncreased;
    public static Action OnStreakEnded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        EnemyBase.OnEnemyDeath += RegisterKill;
    }

    private void OnDisable()
    {
        EnemyBase.OnEnemyDeath -= RegisterKill;
    }

    private void Update()
    {
        if (timerIsRunning)
            timer = streakWindow - (Time.time - startTime);

        Debug.Log(timer);

        if (timerIsRunning && timer <= 0)
            EndStreak();
    }

    private void RegisterKill(int score, EnemyBase enemy)
    {
        if (enemy == previousEnemy) return;
        
        previousEnemy = enemy;

        if (!timerIsRunning) StartStreak();
        else ContinueStreak();

        OnStreakIncreased(currentStreak);
    }

    private void StartStreak()
    {
        currentStreak = 1;
        timerIsRunning = true;
        startTime = Time.time;
    }

    private void ContinueStreak()
    {
        currentStreak++;
        startTime = Time.time;
    }

    private void EndStreak()
    {
        if (timer < 0) timer = 0;
        currentStreak = 0;
        timerIsRunning = false;
        totalStreakBonus = 0;
        OnStreakEnded();
    }
}
