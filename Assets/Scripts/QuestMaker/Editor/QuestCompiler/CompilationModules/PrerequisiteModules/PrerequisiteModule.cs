using QuestMaker.Generic.QuestData;
using QuestMaker.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace QuestMaker.Editor.Compiler.CompilationModules
{
    internal class PrerequisiteModule : IQuestModuleBuilder, IItemModule, ILevelModule
    {
        private int level = 0;
        private int itemAmount = 0;
        private readonly List<QuestSO> quests = new();

        private readonly List<Item> items = new();

        private PrerequisiteData data;

        public void SetQuestPrerequisite(QuestSO quest)
        {
            if (quest == null) throw new NullReferenceException($"[{GetType().Name}] Cannot add quest coz its null");

            quests.Add(quest);

            Debug.Log("Added level prereq= " + level);
        }
        public void SetItemPrerequisite(Item item)
        {
            if (item == null) throw new NullReferenceException($"[{GetType().Name}] Cannot add Item coz its null");

            items.Add(item);
        }

        public void Build(QuestSO quest)
        {
            data = new PrerequisiteData(items, quests, level);
            quest.AddPrerequisites(data);
            Debug.Log($"[PrereqModule] Set the data");
        }

        public void SetLevel(int level)
        {
            if(level < 1) this.level = 1;

            this.level = level;

            Debug.Log($"Level Prereq was set to {level}");
        }

        public void SetItem(Item item, int amount = 1)
        {
            items.Add(item);
            itemAmount = amount;
        }
    }
}
