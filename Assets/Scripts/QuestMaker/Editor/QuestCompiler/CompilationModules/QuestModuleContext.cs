using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuestMaker.Editor.Compiler.CompilationModules
{
    internal class QuestModuleContext
    {
        private Dictionary<Type, IQuestBuildingModule> modules = new();

        public IReadOnlyDictionary<Type, IQuestBuildingModule> Modules => modules;


        public T GetModule<T>() where T : class, IQuestBuildingModule , new()
        {
            Type type = typeof(T);

            if(!modules.TryGetValue(type, out IQuestBuildingModule module))
            {
                module = new T();
                modules.Add(type, module);
                Debug.Log($"Created module of type {type}");            }

            return module as T; 
        }


        public QuestSO Build()
        {
            QuestSO quest = ScriptableObject.CreateInstance<QuestSO>();

            foreach (IQuestBuildingModule module in Modules.Values)
            {
                module.Build(quest);
            }
            return quest;
        }

    }
}   