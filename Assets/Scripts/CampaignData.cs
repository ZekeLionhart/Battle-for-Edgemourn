using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CampaignData", menuName = "Campaign")]
public class CampaignData : ScriptableObject
{
    public List<LevelData> levels;

    public LevelData GetNextLevel(LevelData currentLevel)
    {
        int currentIndex = levels.IndexOf(currentLevel);

        if (currentIndex < 0)
        {
            Debug.LogError("Current level is not part of the campaign.");
            return null;
        }

        int nextIndex = currentIndex + 1;

        if (nextIndex >= levels.Count)
            return null;

        return levels[nextIndex];
    }
}