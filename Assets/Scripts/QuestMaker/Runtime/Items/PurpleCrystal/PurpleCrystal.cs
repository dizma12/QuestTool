using QuestMaker.Domain;
using QuestMaker.Runtime.Events;
using UnityEngine;

namespace QuestMaker.Runtime.Game
{
    public class PurpleCrystal : MonoBehaviour, ICollectable
    {
        [SerializeField] Item item = null;
        public void Collect()
        {
            ReferenceManager.Instance.GetReference<GameEventManager>().RequestBus<GameEventBus>().FireItemCollected(item.ID);
        }
    }
}
