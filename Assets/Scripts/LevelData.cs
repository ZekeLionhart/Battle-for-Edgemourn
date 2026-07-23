using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public string sceneName;

    public Sprite preview;

    public bool unlockedByDefault;

    public int chapter;
}
