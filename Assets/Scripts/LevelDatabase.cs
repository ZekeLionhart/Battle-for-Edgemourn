using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Database", menuName = "Level Database")]
public class LevelDatabase : ScriptableObject
{
    public List<LevelData> levels;
}