using QuestMaker.Data;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace QuestMaker.Editor.Compiler.CompilationModules
{
    internal class PrerequisiteModule : IQuestModuleBuilder, IItemModule, ILevelModule
    {
        private int _level = 0;
        private readonly List<QuestSO> quests = new();

        private readonly List<ItemAmount> items = new();



        public void SetQuestPrerequisite(QuestSO quest)
        {
            if (quest == null) throw new NullReferenceException($"[{GetType().Name}] Cannot add quest coz its null");

            quests.Add(quest);

            Debug.Log("Added level prereq= " + _level);
        }
        public void SetItemPrerequisite(Item item, int amount)
        {
            if (item == null) throw new NullReferenceException($"[{GetType().Name}] Cannot add Item coz its null");

            items.Add(new() { Item = item, Amount = amount });
        }

        public void Build(QuestSO quest)
        {
            PrerequisiteData data = new(items.ToArray(), quests.ToArray(), _level);
            quest.Prerequisites = data;
        }

        public void SetLevel(int level)
        {
            if (level < 1) level = 1;

            _level = level;
        }

        public void SetItem(Item item, int amount = 1)
        {
            if(item != null && amount > 1)
                items.Add(new() { Item = item, Amount = amount });  
        }
    }
}
