using QuestMaker.Data;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace QuestMaker.Editor.Compiler.CompilationModules
{
    internal class PrerequisiteModule : IQuestModuleBuilder, IItemModule, ILevelModule, IReputationModule, IInGameTimeConstraintModule
    {
        private int _level = 0;
        private readonly List<QuestSO> _quests = new();

        private readonly List<ReputationFaction> _reps = new();
        private readonly List<ItemAmount> _items = new();
        private InGameTimeline _inGameTimeConstraint = InGameTimeline.None;

        public void Build(QuestSO quest)
        {
            PrerequisiteData data = new(_items.ToArray(), _quests.ToArray(), _level, _reps.ToArray(), _inGameTimeConstraint);
            quest.Prerequisites = data;
        }

        public void SetQuestPrerequisite(QuestSO quest)
        {
            if (quest == null) throw new NullReferenceException($"[{GetType().Name}] Cannot add quest coz its null");

            _quests.Add(quest);

            Debug.Log("Added level prereq= " + _level);
        }
        public void SetItemPrerequisite(Item item, int amount)
        {
            if (item == null) throw new NullReferenceException($"[{GetType().Name}] Cannot add Item coz its null");

            _items.Add(new() { Item = item, Amount = amount });
        }


        public void SetLevel(int level)
        {
            if (level < 1) level = 1;

            _level = level;
        }

        public void SetItem(Item item, int amount = 1)
        {
            if(item != null && amount > 1)
                _items.Add(new() { Item = item, Amount = amount });  
        }


        public void SetReputationFaction(ReputationFaction rep)
        {
            if (string.IsNullOrEmpty(rep.FactionID) || rep.Amount < 1)
                throw new NullReferenceException($"[{GetType().Name}] Cannot add Reputation coz its null or less than 0");

            _reps.Add(rep);
        }

        public void SetTimeConstraint(InGameTimeline time)
        {
            if(_inGameTimeConstraint != time)
                _inGameTimeConstraint = time;
        }
    }
}
