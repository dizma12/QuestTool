using QuestMaker.Generic.QuestData;
using QuestMaker.Runtime;
using QuestMaker.Runtime.Data;
using System.Collections.Generic;
using UnityEngine;

public class QuestSO : ScriptableObject
{
    public string QuestID { get; set; } = string.Empty;    
    public string QuestName { get; set; } = string.Empty;
    public string QuestDescription { get; init; }

    public int LevelPrereq = 0;
    public List<Item> ItemPrereq = new();
    public List<QuestSO> QuestPrereq = new();
    public PrerequisiteData Prerequisites;
    public void AddPrerequisites(PrerequisiteData data)
    {
        Prerequisites = data;
        Debug.Log($"[QuestSO] Prerequisite Level= {Prerequisites.Level}");
    }
    //[SerializeReference]
    //public List<PrerequisiteData> Prerequisites = null;

    //[SerializeReference]
    //public List<RewardData> Rewards = null;

}
