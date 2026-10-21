using System;
using UnityEngine;

public class HealthManager : MonoBehaviour, IDamageable
{
    public static HealthManager Instance { get; private set; }

    [SerializeField] private AudioSource hitSfx;
    [SerializeField] private int maxHealth;
    private int currentHealth;
    private bool isAlive = true;
    private bool hasGameEnded = false;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    public static event Action<int> OnHealthChanged;
    public static event Action OnZeroHealth;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        GameManager.OnGameEnded += FreezeHealth;
    }

    private void OnDisable()
    {
        GameManager.OnGameEnded -= FreezeHealth;
    }

    private void Start()
    {
        OnHealthChanged(currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive || hasGameEnded) return;

        currentHealth -= damage;
        hitSfx.Play();

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isAlive = false;
            OnZeroHealth();
        }

        OnHealthChanged(currentHealth);
    }

    public void Heal(int amount)
    {
        if (!isAlive || hasGameEnded) return;

        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        OnHealthChanged(currentHealth);
    }

    private void FreezeHealth()
    {
        hasGameEnded = true;
    }
}
