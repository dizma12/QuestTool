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

        public void SetMeta(string id, string desc)
        {
            objectiveID = id;
            description = desc;
        }

        public void AddStep(QuestStepData step)
        {
            if (step != null || steps.Contains(step))
                steps.Add(step);

        }

        public void Build(QuestSO quest)
        {
            quest.AddObjective(new ObjectiveData
            {
                ID = objectiveID,
                Description = description,
                Steps = new List<QuestStepData>(steps)
            });

            Debug.Log($"Building Objective module with step count: {steps.Count}");
        }
    }
}