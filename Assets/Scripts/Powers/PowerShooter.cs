using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PowerShooter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject lineRenderer;
    private bool isPointerInside;
    private bool permissionToShoot;

    public static Action OnScreenClick;
    public static Action<bool> IsInsideArea;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerInside = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerInside = false;
    }

    private void Update()
    {
        if (isPointerInside && Input.GetButtonDown(KeyNames.Fire))
            IsInsideArea(true);

        if (!isPointerInside && Input.GetButtonDown(KeyNames.Fire))
            IsInsideArea(false);
    }
}
