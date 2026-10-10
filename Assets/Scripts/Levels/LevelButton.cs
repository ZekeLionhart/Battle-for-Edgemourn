using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI levelNameBox;
    [SerializeField] private Image lockedOverlay;
    [SerializeField] private Image lockpad;
    [SerializeField] private Image victoryStar;
    [SerializeField] private Image victoryIcon;
    [SerializeField] private Image scoreStar;
    [SerializeField] private Image scoreIcon;
    [SerializeField] private Image defenseStar;
    [SerializeField] private Image defenseIcon;
    [SerializeField] private Image[] numerals;
    private LevelData levelData;
    private LevelProgress levelProgress;

    public static Action<string> OnLevelChosen;

    private void Awake()
    {
        button.onClick.AddListener(StartLevel);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(StartLevel);
    }

    public void Initialize(LevelData level, LevelProgress progress, int levelCount)
    {
        levelData = level;
        levelProgress = progress;
        levelNameBox.text = level.levelName;

        SetSealNumberal(levelCount);
        if (SetLockedState(progress)) return;
        SetEarnedStars(progress);
    }

    private void SetSealNumberal(int levelCount)
    {
        for (int i = 0; i < levelCount; i++)
            numerals[i].gameObject.SetActive(true);
    }

    private bool SetLockedState(LevelProgress progress)
    {
        if (progress.state == LevelStates.Locked) return true;
        else
        {
            lockedOverlay.gameObject.SetActive(false);
            lockpad.gameObject.SetActive(false);
            return false;
        }
    }

    private void SetEarnedStars(LevelProgress progress)
    {
        if (progress.victoryStar)
        {
            victoryStar.color = Color.white;
            victoryIcon.color = Color.white;
        }

        if (progress.scoreStar)
        {
            scoreStar.color = Color.white;
            scoreIcon.color = Color.white;
        }

        if (progress.defenseStar)
        {
            defenseStar.color = Color.white;
            defenseIcon.color = Color.white;
        }
    }

    public void StartLevel()
    {
        if (levelProgress.state != LevelStates.Locked)
            OnLevelChosen(levelData.sceneName);
    }

    public void SetName(string name)
    {
        levelNameBox.text = name;
    }
}
