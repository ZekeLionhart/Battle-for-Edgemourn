using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance { get; private set; }

    [SerializeField] private SaveSystem saveSystem;
    [SerializeField] private CampaignData campaign;
    [SerializeField] private ProgressData progressData;

    public ProgressData CurrentProgress => progressData;
    public IReadOnlyList<LevelData> Levels => campaign.levels;

    private void Awake()
    {
        LoadProgress();
        CreateProgressData();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        GameManager.OnResultCalculated += MergeResultIntoProgress;
    }

    private void OnDisable()
    {
        GameManager.OnResultCalculated -= MergeResultIntoProgress;
    }

    private void LoadProgress()
    {
        progressData = saveSystem.LoadProgress();
    }

    public void SaveProgress()
    {
        saveSystem.SaveProgress(progressData);
    }

    private void CreateProgressData()
    {
        foreach (LevelData level in campaign.levels)
        {
            if (!progressData.levels.Any(progress => progress.levelID == level.levelID))
            {
                LevelProgress progress = new()
                {
                    levelID = level.levelID,
                    state = LevelStates.Locked,
                    stars = 0
                };

                if (level.unlockedByDefault) progress.state = LevelStates.Unlocked;

                progressData.levels.Add(progress);
            }
        }

        SaveProgress();
    }

    private void MergeResultIntoProgress(MatchResult result)
    {
        if (result.victory)
        {
            foreach (LevelProgress progress in progressData.levels)
            {
                if (progress.levelID == result.level.levelID)
                {
                    progress.state = LevelStates.Completed;

                    if (result.stars > progress.stars)
                        progress.stars = result.stars;

                    break;
                }
            }
        }

        progressData.coins += result.coinsEarned;

        UnlockNextLevel(result.level);

        SaveProgress();
    }

    private void UnlockNextLevel(LevelData currentLevel)
    {
        LevelData nextLevel = campaign.GetNextLevel(currentLevel);

        if (nextLevel == null) return;
        
        foreach (LevelProgress progress in progressData.levels)
        {
            if (progress.levelID == nextLevel.levelID)
            {
                if (progress.state == LevelStates.Locked)
                    progress.state = LevelStates.Unlocked;
                break;
            }
        }
    }

    public LevelProgress FindLevelProgress(LevelData level)
    {
        foreach (LevelProgress levelProgress in progressData.levels)
            if (level.levelID == levelProgress.levelID)
                return levelProgress;

        return new LevelProgress();
    }
}
