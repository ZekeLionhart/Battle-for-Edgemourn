using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerShooter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private GameObject lineRenderer;
    private bool isPointerInside;

    public static event Action<bool> IsInsideArea;
    public static event Action<Vector2> OnAimStarted;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerInside = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerInside = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        OnAimStarted?.Invoke(eventData.position);
    }

    private void Update()
    {
        if (isPointerInside && Input.GetButtonDown(KeyNames.Fire))
            IsInsideArea(true);

        if (!isPointerInside && Input.GetButtonDown(KeyNames.Fire))
            IsInsideArea(false);
    }
}
