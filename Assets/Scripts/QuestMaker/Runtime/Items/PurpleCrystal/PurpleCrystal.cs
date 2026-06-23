using QuestMaker.Domain.Interactions;
using QuestMaker.Runtime.Events;
using UnityEngine;

namespace QuestMaker.Runtime.Game
{
    internal class PurpleCrystal : RuntimeItem, ICollectable
    {
        private void Start()
        {
            ID = item.ID;
        }
        public void Collect()
        {
            for (int i = 0; i < 10; i++)
            {
                ReferenceManager.Instance.RequestReference<GameEventManager>().RequestBus<GameEventBus>().FireItemCollected(item.ID);
            }
        }
    }
}
