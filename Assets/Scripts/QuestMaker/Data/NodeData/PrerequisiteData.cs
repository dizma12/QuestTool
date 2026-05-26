using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace QuestMaker.Data
{
    [Serializable]
    public struct PrerequisiteData
    {
        public readonly int Level => _level;
        public readonly IReadOnlyList<ItemAmount> Items => _items;
        public readonly IReadOnlyList<QuestSO> Quests => _quests;

        [SerializeField]
        private int _level;

        [SerializeField]
        private ItemAmount[] _items;

        [SerializeReference]
        private QuestSO[] _quests;

        public PrerequisiteData(ItemAmount[] items, QuestSO[] quests, int level)
        {
            _level = level > 1 ? level : 1;

            _items = items == null || items.Any(i => i.Item == null) ? Array.Empty<ItemAmount>() : items;

            _quests = quests == null || quests.Any(q => q == null) ? Array.Empty<QuestSO>() : quests;
        }
    
    }
}
