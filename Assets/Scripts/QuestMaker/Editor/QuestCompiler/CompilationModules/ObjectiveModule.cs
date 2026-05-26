using QuestMaker.Data;
using QuestMaker.Data.Objectives;
using QuestMaker.Data.Steps;
using System.Collections.Generic;
using UnityEngine;


namespace QuestMaker.Editor.Compiler.CompilationModules
{
    internal class ObjectiveModule : IQuestModuleBuilder
    {
        private readonly List<QuestStepData> steps = new();
        private string objectiveID = string.Empty;
        private string description = string.Empty;

        public void SetDescription(string desc)
        {
            description = desc;
        }

        public void AddStep(QuestStepData step)
        {
            if (step != null || steps.Contains(step))
                steps.Add(step);

        }

        public void Build(QuestSO quest)
        {
            ObjectiveData data = new ObjectiveData
            {
                Description = description,
                Steps = new List<QuestStepData>(steps)
            };
            quest.AddObjective(data);

            Debug.Log($"Building Objective module with step count: {steps.Count} and id: {data.ID}");
        }
    }
}