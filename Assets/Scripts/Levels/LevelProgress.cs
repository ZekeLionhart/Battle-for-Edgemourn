[System.Serializable]
public class LevelProgress
{
    public LevelIDs levelID;
    public LevelStates state = LevelStates.Locked;
    public int stars;
}