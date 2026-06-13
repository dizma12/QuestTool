using QuestMaker.Core.Quests;
using System;
namespace QuestMaker.Data.Steps
{
    [Serializable]
    public abstract class QuestStepData
    {
        public abstract StepCategory StepType { get; }

        //public IRuntimeStep CreateRuntimeStep(GameEventHandler )
    }
}