using System;
using UnityEngine;

public class StreakManager : MonoBehaviour
{
    public static StreakManager Instance { get; private set; }

    [SerializeField] private float streakWindow;
    [SerializeField] private float streakMultiplier;
    private EnemyBase previousEnemy;
    private int currentStreak;
    private float startTime;
    private float timer;
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

    private void Update()
    {
        if (timerIsRunning)
        {
            timer = streakWindow - (Time.time - startTime);
            if (timer <= 0) EndStreak();
        }
    }

    public int RegisterKill(EnemyBase enemy, Transform popupCoord)//tirar o popupCoord?
    {
        if (enemy == previousEnemy) return 0;
        
        previousEnemy = enemy;

        if (!timerIsRunning) StartStreak();
        else ContinueStreak();

        return currentStreak;
    }

    private void StartStreak()
    {
        currentStreak = 1;
        timerIsRunning = true;
        startTime = Time.time;

        OnStreakIncreased(currentStreak);
    }

    private void ContinueStreak()
    {
        currentStreak++;
        startTime = Time.time;

        OnStreakIncreased(currentStreak);
    }

    private void EndStreak()
    {
        if (timer < 0) timer = 0;
        currentStreak = 0;
        timerIsRunning = false;
        OnStreakEnded();
    }
}
