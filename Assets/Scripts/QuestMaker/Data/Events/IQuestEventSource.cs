using System;

namespace QuestMaker.Domain.Events
{
    /// <summary>
    /// Event Bus for Quest related events.
    /// </summary>
    public interface IQuestEventSource
    {
        event Action<string> OnEnemyKilled; // enemy id
        event Action<string> OnItemCollected; // item id
        event Action<string> OnItemCrafted;// item id
        event Action<string> OnAreaEntered; // area id
        event Action<string> OnItemDelivered; // item id
        event Action<string> OnNpcTalked; // npc id
    }
}
