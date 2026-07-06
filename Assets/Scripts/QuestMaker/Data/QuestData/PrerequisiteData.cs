using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace QuestMaker.Domain
{
    [Serializable]
    public class PrerequisiteData
    {
        public int Level => _level;
        public IReadOnlyList<ItemStack> Items => _items;
        public IReadOnlyList<string> Quests => _quests;
        public IReadOnlyList<ReputationFaction> Reputation => _reps;
        public InGameTimeline TimeConstraint => _inGameTimeConstraint;

        [SerializeField]
        private int _level;

        [SerializeField]
        private ItemStack[] _items;

        [SerializeField]
        private ReputationFaction[] _reps;

        [SerializeField]
        private string[] _quests;

        [SerializeField]
        private InGameTimeline _inGameTimeConstraint;


        public PrerequisiteData(ItemStack[] items, string[] quests, int level, ReputationFaction[] rep, InGameTimeline timeConstraint)
        {
            _level = level > 1 ? level : 1;

            _items = items == null || items.Any(i => i.Item == null) ? Array.Empty<ItemStack>() : items;

            _quests = quests == null || quests.Any(q => string.IsNullOrEmpty(q)) ? Array.Empty<string>() : quests;

            _reps = rep == null || rep.Any(r => r.FactionID == string.Empty || r.Amount <= 0)  ? Array.Empty<ReputationFaction>() : rep;

            _inGameTimeConstraint = timeConstraint;
        }

    }
}

