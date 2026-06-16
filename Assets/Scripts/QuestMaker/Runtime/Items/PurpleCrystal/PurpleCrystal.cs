using QuestMaker.Domain;
using QuestMaker.Runtime.Events;
using UnityEngine;

namespace QuestMaker.Runtime.Game
{
    internal class PurpleCrystal : RuntimeItem, ICollectable
    {
        public void Collect()
        {
            for (int i = 0; i < 10; i++)
            {
                ReferenceManager.Instance.GetReference<GameEventManager>().RequestBus<GameEventBus>().FireItemCollected(item.ID);
            }
        }
    }
}
