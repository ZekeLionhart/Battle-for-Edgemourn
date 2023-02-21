using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CooldownManager : MonoBehaviour
{
    [SerializeField] private PowerController power;
    [SerializeField] private RectTransform overlay;
    private bool cooldownStarted = true;
    private float cooldownLength;
    private float startingHeight;

    public static Action<PowerController> OnCooldownEnded;

    private void Awake()
    {
        startingHeight = overlay.rect.size.y;   
    }

    private void OnEnable()
    {
        PowerController.OnPowerShoot += StartCooldown;
    }

    private void OnDisable()
    {
        PowerController.OnPowerShoot -= StartCooldown;
    }

    void Update()
    {
        if (cooldownStarted)
            RunCooldown();
    }

    private void RunCooldown()
    {
        float x = overlay.rect.size.x;
        float y = overlay.rect.size.y;

        if (y >= startingHeight)
        {
            overlay.sizeDelta = new Vector2(x,0);
            cooldownStarted = false;
            OnCooldownEnded(power);
        }
        else
        {
            overlay.sizeDelta += startingHeight * Time.deltaTime * Vector2.up / cooldownLength;
        }

    }

    private void StartCooldown(PowerController power, float length)
    {
        if (power == this.power)
        {
            cooldownStarted = true;
            cooldownLength = length;
        }
    }
}
