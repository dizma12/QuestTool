using QuestMaker.Data;
using QuestMaker.Data.Objectives;
using QuestMaker.Data.SpecialEvents;
using QuestMaker.Data.Steps;
using System.Collections.Generic;
using UnityEngine;


namespace QuestMaker.Editor.Compiler.CompilationModules
{
    internal class ObjectiveModule : IQuestModuleBuilder, ISpecialEventModule
    {
        private readonly List<QuestStepData> _steps = new();
        private List<SpecialEventData> _specialEvents;
        private string objectiveID = string.Empty;
        private string description = string.Empty;

        public void SetDescription(string desc)
        {
            description = desc;
        }

        public void AddStep(QuestStepData step)
        {
            if (step != null && !_steps.Contains(step))
                _steps.Add(step);
        }

        public void Build(QuestSO quest)
        {
            ObjectiveData data = new ObjectiveData
            {
                Description = description,
                Steps = new List<QuestStepData>(_steps)
            };
            quest.AddObjective(data);

            Debug.Log($"Building Objective module with step count: {_steps.Count} and id: {data.ID}");
        }

        public void SetSpecialEvent(SpecialEventData eventData)
        {
            _specialEvents ??= new List<SpecialEventData>();

            if (_specialEvents.Contains(eventData) || eventData.Equals(default)) return;

            _specialEvents.Add(eventData);
        }
    }
}