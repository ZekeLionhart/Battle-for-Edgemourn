using System;
using UnityEngine;

public class CooldownView : MonoBehaviour
{
    [SerializeField] private PowerController power;
    [SerializeField] private RectTransform overlay;
    [SerializeField] private GameObject border;
    [SerializeField] private GameObject crystal;
    private float startingHeight;

    private void Awake()
    {
        startingHeight = overlay.rect.height;   
    }

    private void OnEnable()
    {
        PowerManager.OnSwitchPowers += ActivateBorder;
        RefreshCooldown();
    }

    private void OnDisable()
    {
        PowerManager.OnSwitchPowers -= ActivateBorder;
    }

    private void LateUpdate()
    {
        RefreshCooldown();
    }

    private void RefreshCooldown()
    {
        float progress = power.CooldownProgress;
        bool isComplete = progress >= 1f;
        float height = isComplete ? 0f : startingHeight * progress;

        overlay.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        crystal.SetActive(isComplete);
    }

    private void ActivateBorder(PowerController selectedPower)
    {
        border.SetActive(selectedPower == power);
    }
}
