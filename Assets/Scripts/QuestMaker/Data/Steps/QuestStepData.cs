using QuestMaker.Domain.Events;
using System;
namespace QuestMaker.Domain.Steps
{
    [Serializable]
    public abstract class QuestStepData
    {
        public abstract IRuntimeStep CreateRuntimeStep(IQuestEventSource eventbus);
    }
}