using UnityEngine;
using static UnityEngine.Debug;
namespace Game.Runtime.Events.Handlers
{

    internal class GameEventHandler : CustomEventHandler, IGameEventHandler
    {
        public void ExternalCall()
        {
            Debug.Log("[GameEventHandler] Executing From External call");
        }
    }
}
