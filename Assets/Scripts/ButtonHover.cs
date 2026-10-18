using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private FlagController targetFlag;

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetFlag.SwitchShine(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetFlag.SwitchShine(false);
    }
}