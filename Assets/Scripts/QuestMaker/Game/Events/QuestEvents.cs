using System;

namespace Assets.Scripts.QuestMaker.Game.Events
{
    public class QuestEvents
    {
        public event Action OnQuestFinished;
        
        public void QuestFinished() => OnQuestFinished?.Invoke();
    }
}