using UnityEngine;

public enum UITextKey { 
    GameTitle = 0, 
    GameOver = 4,
    ClickToStart = 1, 
    ClickToRestart = 2, 
    AppVersion = 3
};

public static class TextDB
{
    public const string ClickToStart = "Click to Start";
    public const string ClickToRestart = "Click to Restart";
    public const string TapToStart = "Tap to Start";
    public const string TapToRestart = "Tap to Restart";
    public const string GameTitle = "Battle for Edgemourn";
    public static string AppVersion = "Version: " + Application.version;

    public static string GetTextByKey(UITextKey key)
    {
        switch (key)
        {
#if PLATFORM_ANDROID
            case UITextKey.ClickToStart: return TextDB.TapToStart;
            case UITextKey.ClickToRestart: return TextDB.TapToRestart;
#else
            case UITextKey.ClickToStart: return TextDB.ClickToStart;
            case UITextKey.ClickToRestart: return TextDB.ClickToRestart;
#endif
            case UITextKey.GameTitle: return TextDB.GameTitle;
            case UITextKey.AppVersion: return TextDB.AppVersion;
            default: return "UNDEFINED";
        }
    }
}
