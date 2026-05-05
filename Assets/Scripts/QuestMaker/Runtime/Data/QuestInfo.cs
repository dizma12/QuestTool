using System.Collections.Generic;
using UnityEngine;


namespace QuestMaker.Runtime.Data
{
    /// <summary>
    /// Temporary class used in compilation of the Graph.
    /// </summary>
    public class QuestInfo
    {
        public QuestInfo()
        {
            Prerequisites = new();
            Rewards = new();
        }

        public string QuestID {get; set;} = string.Empty;
        public string QuestName { get; set; } = string.Empty;


        public List<PrerequisiteData> Prerequisites;
        public List<RewardData> Rewards;
    }
}
