using System.Collections.Generic;
using UnityEngine;

public class LevelPack : MonoBehaviour
{
    [SerializeField] private Chapters chapter;
    [SerializeField] private FlagController flag;
    [SerializeField] private ChapterPopupDisplay popupDisplay;
    private List<LevelData> levels = new();
    private LevelStates state = LevelStates.Locked;

    public Chapters Chapter => chapter;

    private void Start()
    {
        CalculateFlagPattern();
    }

    public void AddLevel(LevelData level)
    {
        levels.Add(level);

        CalculateFlagPattern();
    }

    private void CalculateFlagPattern()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            LevelProgress progress = ProgressManager.Instance.FindLevelProgress(levels[i]);

            if (i == 0 && progress.state == LevelStates.Locked)
            {
                state = LevelStates.Locked;
                break;
            }
            else state = LevelStates.Unlocked;

            if (i == levels.Count - 1 && progress.state == LevelStates.Completed)
                state = LevelStates.Completed;
        }

        flag.SetFlagPattern(state);
    }

    public void OpenChapter()
    {
        LevelProgress progress = ProgressManager.Instance.FindLevelProgress(levels[0]);

        if (progress.state != LevelStates.Locked)
            popupDisplay.OpenChapter(levels, chapter);
    }
}
