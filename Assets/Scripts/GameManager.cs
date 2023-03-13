using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject lineRenderer;
    [SerializeField] private List<PowerController> powers = new();
    private List<bool> cooldowns = new();
    private PowerController activePower;

    public static Action<PowerController> OnSwitchPowers;

    private void Awake()
    {
        activePower = powers[0];

        for (int i = 0; i < powers.Count; i++)
            cooldowns.Add(false);
    }

    private void OnEnable()
    {
        HealthManager.OnZeroHealth += FailGame;
        PowerController.OnPowerShoot += StartPowerCooldown;
        CooldownManager.OnCooldownEnded += EndPowerCooldown;
    }

    private void OnDisable()
    {
        HealthManager.OnZeroHealth -= FailGame;
        PowerController.OnPowerShoot -= StartPowerCooldown;
        CooldownManager.OnCooldownEnded -= EndPowerCooldown;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Power1"))
            SwitchPowers(0);
        if (Input.GetButtonDown("Power2"))
            SwitchPowers(1);
        if (Input.GetButtonDown("Power3"))
            SwitchPowers(2);
        if (Input.GetButtonDown("Power4"))
            SwitchPowers(3);
        if (Input.GetButtonDown("Power5"))
            SwitchPowers(4);
        if (Input.GetButtonDown("Power6"))
            SwitchPowers(5);
    }

    private void SwitchPowers(int index)
    {
        lineRenderer.SetActive(index == 0 || index == 1);
        powers[powers.IndexOf(activePower)].gameObject.SetActive(false);

        powers[index].gameObject.SetActive(true);
        activePower = powers[index];

        OnSwitchPowers(activePower);

        if (!cooldowns[index])
            CooldownManager.OnCooldownEnded(powers[index]);
    }

    private void StartPowerCooldown(PowerController power, float cooldown)
    {
        for (int i = 0; i < powers.Count; i++)
        {
            if (power == powers[i])
            {
                cooldowns[i] = true;
                break;
            }
        }
    }

    private void EndPowerCooldown(PowerController power)
    {
        for (int i = 0; i < powers.Count; i++)
        {
            if (power == powers[i])
            {
                cooldowns[i] = false;
                break;
            }
        }
    }

    private void FailGame()
    {
        SceneManager.LoadScene("GameOverScene");
    }
}
