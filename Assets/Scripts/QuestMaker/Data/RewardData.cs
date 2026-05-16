using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuestMaker.Data
{
    [Serializable]
    public struct RewardData
    {
        public readonly int Exp => _exp;

        [SerializeField]
        private int _exp;

        public readonly IReadOnlyList<Item> Items => _items;

        [SerializeReference]
        List<Item> _items;

   

        public RewardData(int exp, List<Item> items)
        {
            _exp = exp;
            _items = items;
        }
    }
}
