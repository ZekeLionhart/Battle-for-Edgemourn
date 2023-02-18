using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CooldownManager : MonoBehaviour
{
    [SerializeField] private PowerManager power;
    [SerializeField] private RectTransform overlay;
    private bool cooldownStarted = true;
    private float cooldownLength;
    private float startingHeight;

    private void Awake()
    {
        startingHeight = overlay.rect.size.y;   
    }

    private void OnEnable()
    {
        PowerManager.OnPowerShoot += StartCooldown;
    }

    private void OnDisable()
    {
        PowerManager.OnPowerShoot -= StartCooldown;
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
        }
        else
        {
            overlay.sizeDelta += startingHeight * Time.deltaTime * Vector2.up / cooldownLength;
        }

    }

    private void StartCooldown(PowerManager power, float length)
    {
        if (power == this.power)
        {
            cooldownStarted = true;
            cooldownLength = length;
        }
    }
}
