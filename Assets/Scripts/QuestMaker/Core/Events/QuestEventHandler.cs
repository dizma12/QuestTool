using Game.Runtime.Events.Handlers;
using QuestMaker.Runtime.StepsAndObjectives;
using System;

namespace QuestMaker.Runtime.Game.Events
{
    public class QuestEventHandler : CustomEventHandler
    {
        public event Action<QuestStep> OnQuestStepFinished;
        
        public void QuestStepFinished(QuestStep step) => OnQuestStepFinished?.Invoke(step);
    }
}