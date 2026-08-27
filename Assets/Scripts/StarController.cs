using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StarController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform star;
    [SerializeField] private AudioSource starHitSFX;
    [SerializeField] private TextMeshProUGUI tooltip;
    [SerializeField] private UITextKey tooltiptext;
    [SerializeField] private float position;
    [SerializeField] private float startX;
    private readonly float endX = 0f;
    private bool canShowTooltip = false;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (canShowTooltip) 
            tooltip.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (canShowTooltip)
            tooltip.gameObject.SetActive(false);
    }

    private void Awake()
    {
        tooltip.text = TextDB.GetTextByKey(tooltiptext);
    }

    private void OnEnable()
    {
        ResultViewer.CallStarDespawn += FadeAway;
    }

    private void OnDisable()
    {
        ResultViewer.CallStarDespawn -= FadeAway;
    }

    private void Update()
    {
        star.localPosition = Vector3.Lerp(
            new Vector3(startX, star.localPosition.y, 0),
            new Vector3(endX, star.localPosition.y, 0),
            position
        );
    }

    private void ActivateTooltips()
    {
        canShowTooltip = true;
        tooltip.gameObject.SetActive(false);
    }

    private void PlayStarHitSFX()
    {
        starHitSFX.Play();
    }

    private void FadeAway()
    {
        animator.SetTrigger(ParameterNames.Disappear);
    }
}
