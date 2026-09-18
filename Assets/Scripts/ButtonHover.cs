using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Chapters chapter;

    public static Action<Chapters, bool> OnFlagHover;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnFlagHover(chapter, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnFlagHover(chapter, false);
    }
}