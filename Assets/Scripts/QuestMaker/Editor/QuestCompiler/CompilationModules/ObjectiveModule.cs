using QuestMaker.Domain;
using QuestMaker.Domain.Objectives;
using QuestMaker.Domain.SpecialEvents;
using QuestMaker.Domain.Steps;
using System.Collections.Generic;

namespace QuestMaker.Editor.CompilationModules
{
    internal class ObjectiveModule : IQuestModuleBuilder, IStepModule, ISpecialEventModule
    {
        private readonly List<QuestStepData> _steps = new();
        private List<SpecialEventData> _specialEvents;
        private string description = string.Empty;

        public void SetDescription(string desc)
        {
            description = desc;
        }

        // IStepModule
        public void AddStep(QuestStepData step)
        {
            if (step != null && !_steps.Contains(step))
                _steps.Add(step);
        }

        // ISpecialEventModule
        public void SetSpecialEvent(SpecialEventData eventData)
        {
            _specialEvents ??= new List<SpecialEventData>();

            if (_specialEvents.Contains(eventData) || eventData.Equals(default)) return;

            _specialEvents.Add(eventData);
        }

        // IQuestModuleBuilder
        public void Build(QuestSO quest)
        {
            ObjectiveData data = new()
            {
                Description = description,
                Steps = new List<QuestStepData>(_steps)
            };

            quest.AddObjective(data);
            ConsoleLogger.Log(this, $"Built objective with {_steps.Count} step(s), ID: {data.ID}");
        }
    }
}