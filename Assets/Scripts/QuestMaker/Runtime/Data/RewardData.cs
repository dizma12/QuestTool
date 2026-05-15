using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuestMaker.Runtime.Data
{
    [Serializable]
    public struct RewardData
    {
        public int Exp { get; private set;}

        public readonly IReadOnlyList<Item> Items => _items; 

        [SerializeReference]
        List<Item> _items;

        public RewardData(int exp, List<Item> items)
        {
            Exp = exp;
            _items = items;
        }
    }
}
