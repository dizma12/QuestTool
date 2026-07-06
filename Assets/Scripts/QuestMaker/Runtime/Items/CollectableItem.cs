using QuestMaker.Domain;
using QuestMaker.Domain.Helpers;
using QuestMaker.Domain.Interactions;
using QuestMaker.Runtime.Events;
using QuestMaker.Runtime.Game;
using UnityEngine;

namespace QuestMaker.Runtime
{
    internal abstract class CollectableItem : MonoBehaviour, ICollectable
    {
        [SerializeField] protected Item _item = null;
        [SerializeField] protected int _amount = 1;
        [SerializeField, ReadOnlyInspector] protected string ID = string.Empty;

        protected virtual void Start()
        {
            ID = _item.ID;
        }

        public virtual void Collect()
        {
            ReferenceManager.Instance.RequestReference<GameEventManager>().RequestBus<GameEventBus>().FireItemCollected(new ItemStack(_item, _amount));
        }
    }
}
