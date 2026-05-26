using System;
using UnityEngine;


namespace QuestMaker.Data
{
    [CreateAssetMenu(menuName = "QuestMaker/Item")]
    [Serializable]
    public class Item : ScriptableObject
    {
        public string ID { get; private set; }
        public string Name { get; private set; }

        [SerializeField] private string id;
        [SerializeField] private string itemName;
        public string ItemName => itemName;
        public Item(string id, string name)
        {
            ID = id;
            Name = name;
        }
    }

    [Serializable]
    public struct ItemAmount
    {
        [SerializeField] private Item _item;
        [SerializeField] private int _amount;
        public Item Item
        {
            readonly get => _item;
            init
            {
                if (value != null)
                    _item = value;
            }
        }
        public int Amount
        {
            readonly get => _amount;
            init
            {
                if (_amount != value && value > 0)
                    _amount = value;
            }
        }
    }
}