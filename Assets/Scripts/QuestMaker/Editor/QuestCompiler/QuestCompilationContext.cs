using QuestMaker.Runtime.Data;
using System;
namespace QuestMaker.Editor.QuestCompiler
{
    internal class QuestCompilationContext
    {
        private QuestData data;
        public QuestData QuestData { get { return data; } }

        public QuestCompilationContext() 
        {
            data = new();
        }

        public QuestCompilationContext(QuestData data)
        {
            this.data = data;
        }

        public void AddQuestInfo(string questid, string questName)
        {
            data.QuestID = questid;
            data.QuestName = questName;
        }

        public void AddQuestPrerequisite(PrerequisiteData prereq)
            => data.Prerequisites.Add(prereq);

        public void AddQuestReward(RewardData reward)
            => data.Rewards.Add(reward);
    }
}
