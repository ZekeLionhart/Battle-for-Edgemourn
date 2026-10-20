using System;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    [SerializeField] private GameObject lineRenderer;
    [SerializeField] private PowerController[] powers;
    private PowerController activePower;
    private bool isReturnToWarbow;

	public PowerController ActivePower => activePower;

	public static event Action<PowerController> OnSwitchPowers;

    private void Awake()
    {
        SetUpStartingPower();
    }

    private void OnEnable()
    {
        PowerController.OnPowerShoot += HandlePowerShot;
        SettingsManager.UpdateSettings += UpdateReturnToBow;
    }

    private void OnDisable()
    {
        PowerController.OnPowerShoot -= HandlePowerShot;
        SettingsManager.UpdateSettings -= UpdateReturnToBow;
    }

    private void Start()
    {
        UpdateReturnToBow(SettingsManager.Instance.CurrentSettings);
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

    private void SetUpStartingPower()
    {
        activePower = powers[0];
        foreach (PowerController power in powers)
            power.SetUpStartingPower(activePower);
    }

    public void SwitchPowers(int index)
    {
        if (!isActiveAndEnabled) return;

        if (activePower != powers[index])
        {
            lineRenderer.SetActive(index == 0 || index == 1);

            activePower = powers[index];

            OnSwitchPowers(activePower);
        }
    }

    private void HandlePowerShot(PowerController power, float cooldown)
    {
        if (isReturnToWarbow && activePower != powers[0])
            SwitchPowers(0);
    }

    private void UpdateReturnToBow(SettingsData data)
    {
        isReturnToWarbow = data.returnToBow;
    }
}
