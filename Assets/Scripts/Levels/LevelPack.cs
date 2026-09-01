using System.Collections.Generic;
using UnityEngine;

public class LevelPack : MonoBehaviour
{
    [SerializeField] private Chapters chapter;
    [SerializeField] private ChapterPopupDisplay popupDisplay;
    private List<LevelData> levels = new();

    public Chapters Chapter => chapter;

    public void AddLevel(LevelData level)
    {
        levels.Add(level);
    }

    public void OpenChapter()
    {
        popupDisplay.OpenChapter(levels);
    }
}
