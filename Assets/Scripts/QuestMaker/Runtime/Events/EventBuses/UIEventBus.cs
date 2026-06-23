using QuestMaker.Runtime.Quests;
using System;
using System.Collections.Generic;

namespace QuestMaker.Runtime.Events
{
    internal class UIEventBus : CustomEventBus
    {
        public event Action<IReadOnlyList<Quest>, IReadOnlyList<Quest>> ShowQuestGiverWindow;
        public event Action CloseQuestGiverWindow;

        public void FireShowQuestGiverWindow(IReadOnlyList<Quest> handins, IReadOnlyList<Quest> turnins)
            => ShowQuestGiverWindow?.Invoke(handins, turnins);

        public void FireCloseQuestGiverWindow()
            => CloseQuestGiverWindow?.Invoke(); 
    }
}
