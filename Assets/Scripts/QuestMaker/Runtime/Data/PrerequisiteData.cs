using QuestMaker.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace QuestMaker.Generic.QuestData
{
    [Serializable]
    public struct PrerequisiteData
    {
        public int Level { get; private set; }
        public readonly IReadOnlyList<Item> Items => items;
        public readonly IReadOnlyList<QuestSO> Quests => quests;

        public PrerequisiteData(List<Item> items, List<QuestSO> quests, int Level)
        {
            this.items = items;
            this.quests = quests;
            this.Level = Level;
        }

        [SerializeReference]
        private List<Item> items;

        [SerializeReference]
        private List<QuestSO> quests;
    }
}
