using System;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private AudioSource hitSfx;
    [SerializeField] private AudioSource deathSfx;
    [SerializeField] private int healthPoints;
    private bool isAlive = true;

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
        if (target.CompareTag(TagNames.Tower) && isAlive)
        { 
            healthPoints -= damage;
            healthText.text = healthPoints.ToString();
            hitSfx.Play();

            if (healthPoints <= 0)
            {
                isAlive = false;
                deathSfx.Play();
                healthText.text = "0";
                OnZeroHealth();
            }
        }
    }
}
