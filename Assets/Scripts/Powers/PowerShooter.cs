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
        if (lineRenderer.activeInHierarchy)
            IsInsideArea(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (lineRenderer.activeInHierarchy) 
            IsInsideArea(false);
    }

    private void ClickScreen()
    {
        OnScreenClick();
    }
}
