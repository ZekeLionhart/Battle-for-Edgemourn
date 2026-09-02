using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterPopupDisplay : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Image popup;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private LevelButton buttonPrefab;
    private List<LevelButton> buttons = new();
    private RectTransform popupRect;
    private ContentSizeFitter popupFitter;

    private void Awake()
    {
        popupRect = popup.GetComponent<RectTransform>();
        popupFitter = popup.GetComponent<ContentSizeFitter>();
    }

    private void OnEnable()
    {
        LevelButton.OnLevelChosen += CloseChapter;
    }

    private void BuildButtons(List<LevelData> newLevels)
    {
        foreach (LevelData level in newLevels)
        {
            LevelButton button = Instantiate(buttonPrefab, buttonContainer);

            LevelProgress progress = ProgressManager.Instance.FindLevelProgress(level);

            button.Initialize(level, progress);
            buttons.Add(button);
        }
    }

    public void OpenChapter(List<LevelData> newLevels)
    {
        BuildButtons(newLevels);

        StartCoroutine(OpenAnimation());
    }

    private IEnumerator OpenAnimation()
    {
        animator.SetTrigger(ParameterNames.OpenPopup);
        popup.gameObject.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(popupRect);

        float targetHeight = popupRect.rect.height;
        float duration = 0.35f;
        float elapsed = 0f;
        Vector2 size = popupRect.sizeDelta;

        popupFitter.enabled = false;
        size.y = 0;
        popupRect.sizeDelta = size;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;
            t = Mathf.SmoothStep(0f, 1f, t);

            SetHeight(Mathf.Lerp(0f, targetHeight, t));

            yield return null;
        }

        SetHeight(targetHeight);
    }

    public void CloseChapter()
    {
        StartCoroutine(CloseAnimation());
    }

    public void CloseChapter(string unused)
    {
        StartCoroutine(CloseAnimation());
    }

    private IEnumerator CloseAnimation()
    {
        animator.SetTrigger(ParameterNames.ClosePopup);

        float startingHeight = popupRect.rect.height;
        float duration = 0.25f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;
            t = Mathf.SmoothStep(0f, 1f, t);

            SetHeight(Mathf.Lerp(startingHeight, 0f, t));

            yield return null;
        }

        SetHeight(0f);

        foreach (LevelButton button in buttons)
            Destroy(button.gameObject);
        buttons.Clear();
        popupFitter.enabled = true;
        popup.gameObject.SetActive(false);
    }

    private void SetHeight(float height)
    {
        Vector2 size = popupRect.sizeDelta;
        size.y = height;
        popupRect.sizeDelta = size;
    }
}
