using System;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance { get; private set; }

    [SerializeField] private AudioSource hitSfx;
    [SerializeField] private AudioSource deathSfx;
    [SerializeField] private int maxHealth;
    private int currentHealth;
    private bool isAlive = true;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    public static Action<int> OnHealthChanged;
    public static Action OnZeroHealth;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        currentHealth = maxHealth;
        OnHealthChanged(currentHealth);
    }

    private void OnEnable()
    {
        EnemyBase.OnDamageDealt += DecreaseHealth;
    }
    private void OnDisable()
    {
        EnemyBase.OnDamageDealt -= DecreaseHealth;
    }

    private void DecreaseHealth(GameObject target, int damage)
    {
        if (target.CompareTag(TagNames.Tower) && isAlive)
        { 
            currentHealth -= damage;
            hitSfx.Play();

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isAlive = false;
                deathSfx.Play();
                OnZeroHealth();
            }

            OnHealthChanged(currentHealth);
        }
    }
}
