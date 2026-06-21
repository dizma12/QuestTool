using QuestMaker.Domain.Interactions;
using QuestMaker.Runtime.Events;
using QuestMaker.Runtime.Game;
using UnityEngine;

namespace QuestMaker.Runtime
{
    internal class YellowCrystal : RuntimeItem, ICollectable
    {
        private void Start()
        {
            ID = item.ID;
        }
        public void Collect()
        {
            for (int i = 0; i < 10; i++)
            {
                ReferenceManager.Instance.GetReference<GameEventManager>().RequestBus<GameEventBus>().FireItemCollected(item.ID);
            }
        }
    }
}
