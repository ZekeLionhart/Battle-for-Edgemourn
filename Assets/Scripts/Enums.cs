public enum DamageTypes { Pierce, Fire, Thunder, Earth }
public enum TargetTypes { Ally, Environment }
public enum AudioTypes { SFX, BGM };

public static class SettingNames
{
    public static string BGM = "BGM";
    public static string SFX = "SFX";
    public static string ReturnToBow = "ReturnToBow";
}

public static class SceneNames
{
    public static string Menu = "MenuScene";
    public static string GameOver = "GameOverScene";
    public static string Level1 = "SampleScene";
}

public static class KeyNames
{
    public static string Pause = "Pause";
    public static string Fire = "Fire1";
    public static string Warbow = "Power1";
    public static string Volley = "Power2";
    public static string Fireball = "Power3";
    public static string Lightning = "Power4";
    public static string Stonewall = "Power5";
    public static string BurningTar = "Power6";
}

public static class TagNames
{
    public static string Player = "Player";
    public static string Enemy = "Enemy";
    public static string Tower = "Tower";
    public static string Floor = "Floor";
}

public static class ParameterNames
{
    public static string StartGame = "Start";
    public static string GameIsOver = "Over";
    public static string IsColliding = "IsColliding";
    public static string OnHpEmpty = "OnHpEmpty";
    public static string OnAttackCldwn = "OnAttackCldwn";
    public static string BowPullWeak = "PullWeak";
    public static string BowPullMed = "PullMed";
    public static string BowPullStrong = "PullStrong";
    public static string Shoot = "Shoot";
    public static string Hit = "Hit";
    public static string Destroy = "Destroy";
}