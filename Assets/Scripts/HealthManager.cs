using System;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private int healthPoints;

    public static Action OnZeroHealth;

    private void Awake()
    {
        healthText.text = healthPoints.ToString();
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
        if (target.CompareTag("Tower"))
        { 
            healthPoints -= damage;
            healthText.text = healthPoints.ToString();

            if (healthPoints <= 0)
            {
                healthText.text = "0";
                OnZeroHealth();
            }
        }
    }
}
