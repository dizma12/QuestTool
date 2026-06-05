using System;
using UnityEngine;


namespace QuestMaker.Data
{
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