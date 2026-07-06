using System;

namespace QuestMaker.Domain.Events
{
    /// <summary>
    /// Event Bus for Quest related events.
    /// </summary>
    public interface IQuestEventSource
    {
        event Action<string> OnEnemyKilled; // enemy id
        event Action<ItemStack> OnItemCollected;
        event Action<ItemStack> OnItemCrafted;
        event Action<string> OnAreaEntered; // area id
        event Action<ItemStack> OnItemDelivered;
        event Action<string> OnNpcTalked; // npc id
    }
}
