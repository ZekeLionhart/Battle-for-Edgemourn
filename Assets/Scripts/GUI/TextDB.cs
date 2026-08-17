using UnityEngine;

public enum UITextKey { 
    AppVersion = 3,
    ClickToStart = 1,
    Continue = 14,
    Defeat = 7,
    GameOver = 4,
    GameSubtitle = 5,
    GameTitle = 0, 
    Morale = 8,
    Quit = 15,
    Restart = 2,
    ScoreEarly = 9,
    ScoreHP = 10,
    ScoreKill = 11,
    ScoreStreak = 12,
    ScoreTotal = 13,
    Victory = 6
};

public static class TextDB
{
    public const string ClickToStart = "Click to Start";
    public const string TapToStart = "Tap to Start";
    public const string Restart = "Restart";
    public const string Continue = "Continue";
    public const string Quit = "Quit";
    public const string GameTitle = "Battle for Edgemourn";
    public const string GameSubtitle = "Chronicles of Archana";
    public static string AppVersion = "Version: " + Application.version;
    public const string Victory = "Victory!";
    public const string Defeat = "Defeat!";
    public const string KillScore = "Kill Score:";
    public const string EarlyScore = "Early Kill Bonus:";
    public const string StreakScore = "Streak Bonus:";
    public const string HPScore = "Defense Bonus:";
    public const string TotalScore = "Final Score:";
    public const string Morale = "Morale:";

    public static string GetTextByKey(UITextKey key)
    {
        switch (key)
        {
#if PLATFORM_ANDROID
            case UITextKey.ClickToStart: return TextDB.TapToStart;
#else
            case UITextKey.ClickToStart: return TextDB.ClickToStart;
#endif
            case UITextKey.Restart: return TextDB.Restart;
            case UITextKey.Continue: return TextDB.Continue;
            case UITextKey.Quit: return TextDB.Quit;
            case UITextKey.GameTitle: return TextDB.GameTitle;
            case UITextKey.GameSubtitle: return TextDB.GameSubtitle;
            case UITextKey.AppVersion: return TextDB.AppVersion;
            case UITextKey.Victory: return TextDB.Victory;
            case UITextKey.Defeat: return TextDB.Defeat;
            case UITextKey.ScoreKill: return TextDB.KillScore;
            case UITextKey.ScoreEarly: return TextDB.EarlyScore;
            case UITextKey.ScoreStreak: return TextDB.StreakScore;
            case UITextKey.ScoreHP: return TextDB.HPScore;
            case UITextKey.ScoreTotal: return TextDB.TotalScore;
            case UITextKey.Morale: return TextDB.Morale;
            default: return "UNDEFINED";
        }
    }
}
