using System.Collections.Generic;

[System.Serializable]
public class ProgressData
{
    public int coins = 0;
    public List<LevelProgress> levels = new();

    public int GetTotalStars()
    {
        int totalStars = 0;

        foreach (LevelProgress level in levels) totalStars += level.stars;

        return totalStars;
    }
}