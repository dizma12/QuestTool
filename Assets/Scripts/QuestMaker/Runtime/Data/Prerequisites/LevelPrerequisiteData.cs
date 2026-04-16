using System;

namespace QuestMaker.Runtime.Data
{
    [Serializable]
    public class LevelPrerequisiteData : PrerequisiteData
    {
        public int Level { get; set; } = 0;
    }
}
