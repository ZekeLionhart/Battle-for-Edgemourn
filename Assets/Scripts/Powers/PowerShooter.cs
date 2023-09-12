using System;
using UnityEngine;

public class PowerShooter : MonoBehaviour
{
    public static Action OnScreenClick;

    public void ClickScreen()
    {
        OnScreenClick();
    }
}
