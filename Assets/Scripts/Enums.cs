using UnityEngine;

public enum DamageTypes { Pierce, Fire, Thunder, Earth }
public enum TargetTypes { Ally, Environment }
public enum AudioTypes { SFX, BGM };
public enum PowerTypes { Warbow, ArrowVolley, Fireball, LightningStrike, Stonewall, BurningTar };

public static class SettingNames
{
    public const string BGM = "BGM";
    public const string SFX = "SFX";
    public const string ReturnToBow = "ReturnToBow";
}

public static class SceneNames
{
    public const string Menu = "MenuScene";
    public const string GameOver = "GameOverScene";
    public const string Level1 = "SampleScene";
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
    public const string GameIsOver = "Over";
    public const string IsColliding = "IsColliding";
    public const string OnHpEmpty = "OnHpEmpty";
    public const string OnAttackCldwn = "OnAttackCldwn";
    public const string BowPullWeak = "PullWeak";
    public const string BowPullMed = "PullMed";
    public const string BowPullStrong = "PullStrong";
    public const string Shoot = "Shoot";
    public const string Hit = "Hit";
    public const string Destroy = "Destroy";
}