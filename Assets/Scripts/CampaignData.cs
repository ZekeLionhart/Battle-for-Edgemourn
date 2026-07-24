using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CampaignData", menuName = "Campaign")]
public class CampaignData : ScriptableObject
{
    public List<LevelData> levels;
}