using System;

namespace QuestMaker.Runtime.Data
{
    [Serializable]
    public class QuestPrerequisiteData : PrerequisiteData
    {
        public string QuestID { get; set; } = default;
    }
}
