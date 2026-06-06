using QuestMaker.Data;
using System;
using UnityEngine;
using static UnityEngine.Debug;
namespace QuestMaker.Runtime.Events.Handlers
{

    internal class GameEventHandler : CustomEventHandler, IGameEventHandler
    {
        public event Action<string> OnEnemyKilled;
        public event Action<string> OnItemCollected;
        public void ExternalCall()
        {
            Debug.Log("[GameEventHandler] Executing From External call");
        }
    }
}
