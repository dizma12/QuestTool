using QuestMaker.Runtime.StepsAndObjectives;
using System;

namespace QuestMaker.Runtime.Events.Handlers
{
    public class QuestEventHandler : CustomEventHandler
    {
        public event Action<QuestStep> QuestStepFinished;
        public event Action<string> SpecialQuestEvent;

        public void FireOnQuestStepFinished(QuestStep step)
        {
            if(step == null) return;

            QuestStepFinished?.Invoke(step);
        }
        public void FireOnPecialQuestEvent(string eventID)
        {
            if(string.IsNullOrEmpty(eventID)) return;

            SpecialQuestEvent?.Invoke(eventID);
        }
    }
}