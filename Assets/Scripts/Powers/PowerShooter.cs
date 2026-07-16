using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerShooter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject lineRenderer;

    public static Action OnScreenClick;
    public static Action<bool> IsInsideArea;

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsInsideArea(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsInsideArea(false);
    }
}
