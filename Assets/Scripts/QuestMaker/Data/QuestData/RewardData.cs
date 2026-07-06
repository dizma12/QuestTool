using System;
using System.Collections.Generic;
using System.Linq;
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
        public IReadOnlyList<ItemStack> Items => _items;
        public IReadOnlyList<string> AbilityIDs => _abilities;
        public IReadOnlyList<ReputationFaction> Reputation => _reps;

        [SerializeField]
        protected int _exp;

        [SerializeField]
        protected ItemStack[] _items;

        [SerializeField]
        protected string[] _abilities;

        [SerializeField]
        protected ReputationFaction[] _reps;

        //public readonly string[] AbilityIDs => _abilities;

        //[SerializeField]
        //private string[] _abilities;

        public RewardData(int exp, ItemStack[] items, string[] abilities, ReputationFaction[] rep)
        {
            _exp = exp > 0 ? exp : 0;

            _items = items == null || items.Any(i => i.Item == null) ? Array.Empty<ItemStack>() : items;

            _abilities = abilities == null || abilities.Any(i => i == null) ? Array.Empty<string>() : abilities;

            _reps = rep == null || rep.Any(r => r.FactionID == string.Empty || r.Amount <= 0) ? Array.Empty<ReputationFaction>() : rep;
        }
    }
}
