using QuestMaker.Runtime.Data;
using System;
using UnityEngine;
namespace QuestMaker.Editor.Compiler
{
    [Serializable]
    internal class QuestCompilationContext
    {
        private QuestInfo data;
        public QuestInfo QuestData { get => data; } 

        public QuestCompilationContext() 
        {
            data = new();
        }

        public QuestCompilationContext(QuestInfo data)
        {
            this.data = data;
        }

        public void AddQuestInfo(string questid, string questName)
        {
            data.QuestID = questid;
            data.QuestName = questName;
        }

        //public void AddQuestPrerequisite(PrerequisiteData prereq)
        //{
        //    data.Prerequisites.Add(prereq);
        //    Debug.Log($"Added prerecquisite of type {prereq.GetType()}");
        //}

        //public void AddQuestReward(RewardData reward)
        //    => data.Rewards.Add(reward);
    }
}
