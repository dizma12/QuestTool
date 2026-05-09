
using QuestMaker.Runtime.Game;
using QuestMaker.Runtime.Game.Events;
using UnityEngine;

namespace Game.Runtime.Events.Handlers
{
    public abstract class CustomEventHandler
    {
        protected GameEventManager manager { get; } = null;
        protected CustomEventHandler()
        {
            manager = ReferenceManager.Instance.GetReference<GameEventManager>();
            if (manager == null)
            {
                Debug.LogWarning($"[{GetType()}] Could not find Reference for GameEventManager");
                return;
            }
        }   
    }
}
