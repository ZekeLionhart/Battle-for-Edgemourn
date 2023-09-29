using System;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    [SerializeField] private GameObject lineRenderer;
    [SerializeField] private PowerController[] powers;
    private readonly List<bool> cooldowns = new();
    private PowerController activePower;
    private bool isReturnToWarbow;

    public static Action<PowerController> SetUpStartingPower;
    public static Action<PowerController> OnSwitchPowers;

    private void Awake()
    {
        SetUpStartingPower(activePower = powers[0]);
        UpdateReturnToBow();

        for (int i = 0; i < powers.Length; i++)
            cooldowns.Add(false);
    }
    private void OnEnable()
    {
        PowerController.OnPowerShoot += StartPowerCooldown;
        CooldownManager.OnCooldownEnded += EndPowerCooldown;
        PowerClick.OnPowerSelect += SwitchPowers;
        SettingsManager.UpdateSettings += UpdateReturnToBow;
    }

    private void OnDisable()
    {
        PowerController.OnPowerShoot -= StartPowerCooldown;
        CooldownManager.OnCooldownEnded -= EndPowerCooldown;
        PowerClick.OnPowerSelect -= SwitchPowers;
        SettingsManager.UpdateSettings -= UpdateReturnToBow;
    }

    private void Update()
    {
        if (Input.GetButtonDown(KeyNames.Warbow))
            SwitchPowers(0);
        if (Input.GetButtonDown(KeyNames.Volley))
            SwitchPowers(1);
        if (Input.GetButtonDown(KeyNames.Fireball))
            SwitchPowers(2);
        if (Input.GetButtonDown(KeyNames.Lightning))
            SwitchPowers(3);
        if (Input.GetButtonDown(KeyNames.Stonewall))
            SwitchPowers(4);
        if (Input.GetButtonDown(KeyNames.BurningTar))
            SwitchPowers(5);
    }

    private void SwitchPowers(int index)
    {
        if (activePower != powers[index])
        {
            lineRenderer.SetActive(index == 0 || index == 1);

            activePower = powers[index];

            OnSwitchPowers(activePower);

            if (!cooldowns[index])
                CooldownManager.OnCooldownEnded(powers[index]);
        }
    }

    private void StartPowerCooldown(PowerController power, float cooldown)
    {
        if (isReturnToWarbow && activePower != powers[0])
            SwitchPowers(0);

        for (int i = 0; i < powers.Length; i++)
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
        for (int i = 0; i < powers.Length; i++)
        {
            if (power == powers[i])
            {
                cooldowns[i] = false;
                break;
            }
        }
    }

    private void UpdateReturnToBow()
    {
        isReturnToWarbow = Convert.ToBoolean(PlayerPrefs.GetInt(SettingNames.ReturnToBow));
    }
}
