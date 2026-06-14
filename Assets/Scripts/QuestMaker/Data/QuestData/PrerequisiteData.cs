using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace QuestMaker.Domain
{
    [Serializable]
    public class PrerequisiteData
    {
        public int Level => _level;
        public IReadOnlyList<ItemAmount> Items => _items;
        public IReadOnlyList<QuestSO> Quests => _quests;
        public IReadOnlyList<ReputationFaction> Reputation => _reps;
        public InGameTimeline TimeConstraint => _inGameTimeConstraint;

        [SerializeField]
        private int _level;

        [SerializeField]
        private ItemAmount[] _items;

        [SerializeField]
        private ReputationFaction[] _reps;

        [SerializeReference]
        private QuestSO[] _quests;

        [SerializeField]
        private InGameTimeline _inGameTimeConstraint;


        public PrerequisiteData(ItemAmount[] items, QuestSO[] quests, int level, ReputationFaction[] rep, InGameTimeline timeConstraint)
        {
            _level = level > 1 ? level : 1;

            _items = items == null || items.Any(i => i.Item == null) ? Array.Empty<ItemAmount>() : items;

            _quests = quests == null || quests.Any(q => q == null) ? Array.Empty<QuestSO>() : quests;

            _reps = rep == null || rep.Any(r => r.FactionID == string.Empty || r.Amount <= 0)  ? Array.Empty<ReputationFaction>() : rep;

            _inGameTimeConstraint = timeConstraint;
        }

    }
}

