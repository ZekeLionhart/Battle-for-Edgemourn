using System;
using UnityEngine;

public class PowerClick : MonoBehaviour
{
    public static Action<int> OnPowerSelect;

    public void OnPowerClick(int index)
    {
        OnPowerSelect(index);
    }
}
