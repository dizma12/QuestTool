using System;
using System.Collections.Generic;
using UnityEngine;


namespace QuestMaker.Data
{
    [Serializable]
    public struct PrerequisiteData
    {
        public readonly int Level => level;
        public readonly IReadOnlyList<Item> Items => items;
        public readonly IReadOnlyList<QuestSO> Quests => quests;

        [SerializeField]
        private int level;

        [SerializeReference]
        private List<Item> items;

        [SerializeReference]
        private List<QuestSO> quests;

        public PrerequisiteData(List<Item> items, List<QuestSO> quests, int Level)
        {
            this.items = items;
            this.quests = quests;
            this.level = Level;
        }
    
    }
}
