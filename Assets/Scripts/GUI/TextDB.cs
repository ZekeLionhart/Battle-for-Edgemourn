using UnityEngine;

public enum UITextKey { 
    AppVersion = 3,
    ClickToStart = 1,
    Continue = 14,
    Defeat = 7,
    GameOver = 4,
    GameSubtitle = 5,
    GameTitle = 0,
    Level1 = 18,
    Level2 = 19,
    Level3 = 20, 
    Morale = 8,
    PerfectDefense = 17,
    Quit = 15,
    Restart = 2,
    ScoreEarly = 9,
    ScoreHP = 10,
    ScoreGoal = 16,
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
    public const string Chapter1 = "Outer Gates";
    public const string Chapter2 = "First Layer";
    public const string Chapter3 = "Middle Layer";
    public const string Chapter4 = "Inner Layer";
    public const string Level1 = "Dawn";
    public const string Level2 = "Noon";
    public const string Level3 = "Dusk";
    public const string Victory = "Victory!";
    public const string Defeat = "Defeat!";
    public const string ScoreGoal = "Score Goal!";
    public const string PerfectDefense = "Perfect Defense!";
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
            case UITextKey.Level1: return TextDB.Level1;
            case UITextKey.Level2: return TextDB.Level2;
            case UITextKey.Level3: return TextDB.Level3;
            case UITextKey.Victory: return TextDB.Victory;
            case UITextKey.Defeat: return TextDB.Defeat;
            case UITextKey.ScoreGoal: return TextDB.ScoreGoal;
            case UITextKey.PerfectDefense: return TextDB.PerfectDefense;
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
