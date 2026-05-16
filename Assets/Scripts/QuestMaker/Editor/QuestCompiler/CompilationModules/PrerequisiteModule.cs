using QuestMaker.Data;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace QuestMaker.Editor.Compiler.CompilationModules
{
    internal class PrerequisiteModule : IQuestModuleBuilder, IItemModule, ILevelModule
    {
        private int _level = 0;
        private int itemAmount = 0;
        private readonly List<QuestSO> quests = new();

        private readonly List<Item> items = new();

        

        public void SetQuestPrerequisite(QuestSO quest)
        {
            if (quest == null) throw new NullReferenceException($"[{GetType().Name}] Cannot add quest coz its null");

            quests.Add(quest);

            Debug.Log("Added level prereq= " + _level);
        }
        public void SetItemPrerequisite(Item item)
        {
            if (item == null) throw new NullReferenceException($"[{GetType().Name}] Cannot add Item coz its null");

            items.Add(item);
        }

        public void Build(QuestSO quest)
        {
            PrerequisiteData data = new (items, quests, _level);
            quest.AddPrerequisites(data);
        }

        public void SetLevel(int level)
        {
            if(level < 1) _level = 1;

            _level = level;
        }

        public void SetItem(Item item, int amount = 1)
        {
            items.Add(item);
            itemAmount = amount;
        }
    }
}
