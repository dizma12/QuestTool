using QuestMaker.Editor.Compiler.CompilationModules;
using QuestMaker.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace QuestMaker.Editor.Compiler.CompilationModules
{ 
    internal class PrerequisiteModule : IQuestBuildingModule
    {
        private int _levelPrereq = 0;

        private readonly List<QuestSO> _questPrereq = new();

        private readonly List<Item> _itemPrereq = new();


        public void SetLevelPrerequisite(int lvl) => _levelPrereq = lvl;
        public void SetQuestPrerequisite(QuestSO quest)
        {
            if (quest == null) throw new NullReferenceException($"[{GetType().Name}] Cannot add quest coz its null");

            _questPrereq.Add(quest);

            Debug.Log("Added level prereq= " + _levelPrereq);
        }
        public void SetItemPrerequisite(Item item)
        {
            {
                if (item == null) throw new NullReferenceException($"[{GetType().Name}] Cannot add Item coz its null");

                _itemPrereq.Add(item);
            }
        }

        public void Build(QuestSO quest)
        {
            quest.LevelPrereq = _levelPrereq;
            quest.QuestPrereq = _questPrereq;
            quest.ItemPrereq = _itemPrereq;
        }
    }
}
