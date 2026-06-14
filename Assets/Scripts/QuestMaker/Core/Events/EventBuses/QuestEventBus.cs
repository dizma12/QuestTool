using QuestMaker.Runtime.Quests;
using System;

namespace QuestMaker.Runtime.Events
{
    public class QuestEventBus : CustomEventBus
    {
        public event Action<string> OnSpecialEvent;
        public event Action<Quest> OnQuestCompleted;

        public void FireSpecialEvent(string eventID)
        {
            if (string.IsNullOrEmpty(eventID)) return;

            OnSpecialEvent?.Invoke(eventID);
        }

        public void FireQuestCompleted(Quest quest)
        {
            if (quest == null) return;

            OnQuestCompleted?.Invoke(quest);
        }
    }
}
