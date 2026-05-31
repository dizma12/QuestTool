using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QuestMaker.Data
{
    /// <summary>
    /// Data struct for Rewards. ***Warning dont use the empty constructor***
    /// </summary>
    [Serializable]
    public struct RewardData
    {
        public readonly int Exp => _exp;

        [SerializeField]
        private int _exp;

        public readonly IReadOnlyList<ItemAmount> Items => _items;

        [SerializeField]
        private ItemAmount[] _items;

        public readonly IReadOnlyList<string> AbilityIDs => _abilities;

        [SerializeField]
        private string[] _abilities;

        //public readonly string[] AbilityIDs => _abilities;

        //[SerializeField]
        //private string[] _abilities;

        public RewardData(int exp, ItemAmount[] items, string[] abilities)
        {
            _exp = exp > 0 ? exp : 0;

            _items = items == null || items.Any(i => i.Item == null) ? Array.Empty<ItemAmount>() : items;
            
            _abilities = abilities == null || abilities.Any(i => i == null) ? Array.Empty<string>() : abilities;
        }
    }
}
