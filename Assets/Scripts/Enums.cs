public enum DamageTypes { Pierce, Fire, Thunder, Earth }
public enum TargetTypes { Ally, Environment }
public enum AudioTypes { SFX, BGM, UISFX};
public enum PowerTypes { Warbow, ArrowVolley, Fireball, LightningStrike, Stonewall, BurningTar };
public enum LevelStates { Locked, Unlocked, Completed };
public enum LevelIDs { OuterGates001 = 0, OuterGates002 = 1, OuterGates003 = 2,
                       FirstLayer001 = 3, FirstLayer002 = 4, FirstLayer003 = 5,
                       MidLayer001 = 6, MidLayer002 = 7, MidLayer003 = 8,
                       InnerLayer001 = 9, InnerLayer002 = 10, InnerLayer003 = 11,
                       EndlessMode = 12 };
public enum Chapters { Tutorial, OuterWalls, FirstLayer, MidLayer, InnerLayer };

public static class SettingNames
{
    public const string BGM = "BGM";
    public const string SFX = "SFX";
    public const string MuteAudio = "MuteAudio";
    public const string ReturnToBow = "ReturnToBow";
    public const string AimStyle = "AimStyle";
    public const string ScreenShake = "ScreenShake";
}

public static class SceneNames
{
    public const string Menu = "MenuScene";
    public const string GameOver = "GameOverScene";
    public const string LevelSelector = "Lv_Selector_Scene";
    public const string LevelInfinite = "SampleScene";
    public const string RestartTransition = "TransitionScene";
}

public static class KeyNames
{
    public const string Pause = "Pause";
    public const string Fire = "Fire1";
    public const string Warbow = "Power1";
    public const string Volley = "Power2";
    public const string Fireball = "Power3";
    public const string Lightning = "Power4";
    public const string Stonewall = "Power5";
    public const string BurningTar = "Power6";
}

public static class TagNames
{
    public const string Player = "Player";
    public const string Enemy = "Enemy";
    public const string Tower = "Tower";
    public const string Floor = "Floor";
}

public static class ParameterNames
{
    public const string StartGame = "Start";
    public const string MatchEnded = "MatchEnd";
    public const string Restart = "Restart";
    public const string Quit = "Quit";
    public const string IsColliding = "IsColliding";
    public const string OnHpEmpty = "OnHpEmpty";
    public const string OnAttackCldwn = "OnAttackCldwn";
    public const string BowPullWeak = "PullWeak";
    public const string BowPullMed = "PullMed";
    public const string BowPullStrong = "PullStrong";
    public const string Shoot = "Shoot";
    public const string Hit = "Hit";
    public const string Destroy = "Destroy";
    public const string Increase = "Increase";
    public const string Pause = "Pause";
    public const string Unpause = "Unpause";
    public const string Spawn = "Spawn";
    public const string Disappear = "Disappear";
    public const string LevelChosen = "LevelChosen";
    public const string OpenPopup = "OpenPopup";
    public const string ClosePopup = "ClosePopup";
}