using QuestMaker.Domain;
using QuestMaker.Domain.Events;
using System;

namespace QuestMaker.Runtime.Events
{
    internal class GameEventBus : CustomEventBus, IQuestEventSource
    {
        public event Action<string> OnEnemyKilled;
        public event Action<ItemStack> OnItemCollected;
        public event Action<ItemStack> OnItemCrafted;
        public event Action<string> OnAreaEntered;
        public event Action<ItemStack> OnItemDelivered;
        public event Action<string> OnNpcTalked;

        public void FireEnemyKilled(string enemyID) => OnEnemyKilled?.Invoke(enemyID);

        public void FireItemCollected(ItemStack stack)
        {
            if (stack.Item == null || stack.Amount <= 0) return;
            OnItemCollected?.Invoke(stack);
        }

        public void FireItemCrafted(ItemStack stack)
        {
            if (stack.Item == null || stack.Amount <= 0) return;
            OnItemCrafted?.Invoke(stack);
        }

        public void FireAreaEntered(string areaID) => OnAreaEntered?.Invoke(areaID);

        public void FireItemDelivered(ItemStack stack)
        {
            if (stack.Item == null || stack.Amount <= 0) return;
            OnItemDelivered?.Invoke(stack);
        }

        public void FireNpcTalked(string npcID) => OnNpcTalked?.Invoke(npcID);
    }
}
