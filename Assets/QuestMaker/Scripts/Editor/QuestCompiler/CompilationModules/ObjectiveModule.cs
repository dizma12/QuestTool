using QuestMaker.Domain;
using QuestMaker.Domain.Objectives;
using QuestMaker.Domain.Steps;
using System.Collections.Generic;

namespace QuestMaker.Editor.CompilationModules
{
    internal class ObjectiveModule : IQuestModuleBuilder, IStepModule
    {
        private readonly List<QuestStepData> _steps = new();
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