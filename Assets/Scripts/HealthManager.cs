using System;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private AudioSource hitSfx;
    [SerializeField] private AudioSource deathSfx;
    [SerializeField] private int maxHealth;
    private int currentHealth;
    private bool isAlive = true;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

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
        healthText.text = currentHealth.ToString();
    }

    private void OnEnable()
    {
        EnemyBase.OnDamageDealt += SubtractHealth;
    }
    private void OnDisable()
    {
        EnemyBase.OnDamageDealt -= SubtractHealth;
    }

    private void SubtractHealth(GameObject target, int damage)
    {
        if (target.CompareTag(TagNames.Tower) && isAlive)
        { 
            currentHealth -= damage;
            healthText.text = currentHealth.ToString();
            hitSfx.Play();

            if (currentHealth <= 0)
            {
                isAlive = false;
                deathSfx.Play();
                healthText.text = "0";
                OnZeroHealth();
            }
        }
    }
}
