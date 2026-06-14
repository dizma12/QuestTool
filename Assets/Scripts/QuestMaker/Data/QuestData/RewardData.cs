using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace QuestMaker.Domain
{
    /// <summary>
    /// Data struct for Rewards. ***Warning dont use the empty constructor***
    /// </summary>
    [Serializable]
    public class RewardData
    {
        public int Exp => _exp;
        public IReadOnlyList<ItemAmount> Items => _items;
        public IReadOnlyList<string> AbilityIDs => _abilities;
        public IReadOnlyList<ReputationFaction> Reputation => _reps;
        [SerializeField]
        private int _exp;


        [SerializeField]
        private ItemAmount[] _items;


        [SerializeField]
        private string[] _abilities;

        [SerializeField]
        private ReputationFaction[] _reps;

        //public readonly string[] AbilityIDs => _abilities;

        //[SerializeField]
        //private string[] _abilities;

        public RewardData(int exp, ItemAmount[] items, string[] abilities, ReputationFaction[] rep)
        {
            _exp = exp > 0 ? exp : 0;

            _items = items == null || items.Any(i => i.Item == null) ? Array.Empty<ItemAmount>() : items;

            _abilities = abilities == null || abilities.Any(i => i == null) ? Array.Empty<string>() : abilities;

            _reps = rep == null || rep.Any(r => r.FactionID == string.Empty || r.Amount <= 0) ? Array.Empty<ReputationFaction>() : rep;
        }
    }
}
