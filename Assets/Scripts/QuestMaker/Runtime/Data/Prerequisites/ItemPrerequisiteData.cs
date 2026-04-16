using System;

namespace QuestMaker.Runtime.Data.QuestPrerequisites
{
    [Serializable]
    public class ItemPrerequisiteData : PrerequisiteData
    {
        public int Amount { get; set; } = 1;
        public Item Item { get; set; } = null;
    }
}
