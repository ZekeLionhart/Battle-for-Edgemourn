using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterPopupDisplay : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private Image popup;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private LevelButton buttonPrefab;
    private List<LevelButton> buttons = new();

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
        background.gameObject.SetActive(true);
        popup.gameObject.SetActive(true);
        BuildButtons(newLevels);
    }

    public void CloseChapter()
    {
        foreach (LevelButton button in buttons)
            Destroy(button.gameObject);
        buttons.Clear();
        background.gameObject.SetActive(false);
        popup.gameObject.SetActive(false);
    }
}
