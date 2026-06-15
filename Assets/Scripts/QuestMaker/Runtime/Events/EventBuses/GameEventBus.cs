using QuestMaker.Domain.Events;
using System;

namespace QuestMaker.Runtime.Events
{
    internal class GameEventBus : CustomEventBus, IQuestEventSource
    {
        public event Action<string> OnEnemyKilled;
        public event Action<string> OnItemCollected;
        public event Action<string> OnItemCrafted;
        public event Action<string> OnAreaEntered;
        public event Action<string> OnItemDelivered;
        public event Action<string> OnNpcTalked;

        public void FireEnemyKilled(string enemyID) => OnEnemyKilled?.Invoke(enemyID);
        public void FireItemCollected(string itemID) => OnItemCollected?.Invoke(itemID);
        public void FireItemCrafted(string itemID) => OnItemCrafted?.Invoke(itemID);
        public void FireAreaEntered(string areaID) => OnAreaEntered?.Invoke(areaID);
        public void FireItemDelivered(string itemID) => OnItemDelivered?.Invoke(itemID);
        public void FireNpcTalked(string npcID) => OnNpcTalked?.Invoke(npcID);
    }
}
