using System;
using UnityEngine;

public class PowerClick : MonoBehaviour
{
    [SerializeField] private PowerManager powerManager;

    public void OnPowerClick(int index)
    {
        powerManager.SwitchPowers(index);
    }
}
