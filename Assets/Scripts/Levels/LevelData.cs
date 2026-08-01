using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level")]
public class LevelData : ScriptableObject
{
    public LevelIDs levelID;
    public string levelName;
    public string sceneName;

    public Image thumbnail;

    public bool unlockedByDefault;

    public int chapter;
}
