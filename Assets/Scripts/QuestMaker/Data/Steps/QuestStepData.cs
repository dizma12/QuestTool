using System;
namespace QuestMaker.Data.Steps
{
    [Serializable]
    public abstract class QuestStepData
    {
        public abstract StepCategory StepType { get; }
    }

    public static class QuestDataExtensions
    {
        public static T GetSubClassData<T>(this QuestStepData data) where T : QuestStepData
        {
            if(data == null || data is not T) return null;

            return data as T;
        }
    }

}