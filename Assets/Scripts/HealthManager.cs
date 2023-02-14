using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private int healthPoints;

    private void Awake()
    {
        healthText.text = healthPoints.ToString();
    }

    private void OnEnable()
    {
        Enemy.OnDamageDealt += SubtractHealth;
    }
    private void OnDisable()
    {
        Enemy.OnDamageDealt -= SubtractHealth;
    }

    private void SubtractHealth(int damage)
    {
        healthPoints -= damage;
        healthText.text = healthPoints.ToString();
    }
}
