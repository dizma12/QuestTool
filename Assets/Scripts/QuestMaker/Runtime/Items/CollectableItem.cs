using QuestMaker.Domain;
using QuestMaker.Domain.Helpers;
using QuestMaker.Domain.Interactions;
using QuestMaker.Runtime.Events;
using QuestMaker.Runtime.Game;
using UnityEngine;

internal class CollectableItem : MonoBehaviour, ICollectable
{
    [SerializeField, ReadOnlyInspector] protected string ID = string.Empty;
    [SerializeField, ReadOnlyInspector] protected Item _item = null;

    [Tooltip("How many items a pickup will will add. EX -> if 2: every pickup will award 2 items")]
    [SerializeField] protected int _itemsPerPickUp = 1;


    public Item Item { get => _item; set { if (_item == null && value != null) _item = value; } }

    /// <summary>
    /// How many items a pickup will will add. EX -> if 2: every pickup will award 2 items
    /// </summary>
    public int ItemsPerPickup { get => _itemsPerPickUp; set { if (value >= 1 && value != _itemsPerPickUp) _itemsPerPickUp = value; } }
    

    protected virtual void Start()
    {
        ID = _item.ID;
        ValidateItemsPerPickUp();
    }

    public virtual void Collect()
    {
        ReferenceManager.Instance.RequestReference<GameEventManager>().RequestBus<GameEventBus>().FireItemCollected(new ItemStack(_item, _itemsPerPickUp));
    }

    private void ValidateItemsPerPickUp()
    {
        if (_itemsPerPickUp == 0)
            _itemsPerPickUp = 1;
        if (_itemsPerPickUp < 0)
            _itemsPerPickUp = Mathf.Abs(_itemsPerPickUp);
    }
}