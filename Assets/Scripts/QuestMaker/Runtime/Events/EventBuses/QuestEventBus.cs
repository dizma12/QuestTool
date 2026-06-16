using QuestMaker.Runtime.Quests;
using System;

namespace QuestMaker.Runtime.Events
{
    internal class QuestEventBus : CustomEventBus
    {
        public event Action<string> OnSpecialEvent;

        public event Action<Quest> OnQuestStarted;
        public event Action<Quest> OnQuestCanFinish;
        public event Action<Quest> OnQuestCompleted;


        public void FireSpecialEvent(string eventID)
        {
            if (string.IsNullOrEmpty(eventID)) return;

            OnSpecialEvent?.Invoke(eventID);
        }
        public void FireQuestStarted(Quest quest)
        {
            if (quest == null) return;

            OnQuestStarted?.Invoke(quest);
        }
        public void FireQuestCanFinish(Quest quest)
        {
            if (quest == null) return;
            OnQuestCanFinish?.Invoke(quest);
        }
        public void FireQuestCompleted(Quest quest)
        {
            if (quest == null) return;

            OnQuestCompleted?.Invoke(quest);
        }
    }
}
