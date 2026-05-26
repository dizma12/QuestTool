using QuestMaker.Data.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QuestMaker.Data.Objectives
{
    [Serializable]
    public class ObjectiveData
    {

        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(id))
                {
                    if (steps != null && steps.Count > 0)
                    {
                        var x = steps.Select(s => s.StepType.ToString());
                        id = string.Join("_", x) + $"_{steps.Count}";
                    }
                }
                return id;
            }

        }
        public string Description
        {
            get => desc;
            set
            {
                if (string.IsNullOrEmpty(desc))
                    desc = value;
            }
        }

        [SerializeField, HideInInspector] private string id = string.Empty;
        [SerializeField] private string desc = string.Empty;

        [SerializeReference]
        private List<QuestStepData> steps = null;

        public IReadOnlyList<QuestStepData> Steps
        {
            get => steps;
            set
            {
                if ((steps == null || !steps.Any()) && value != null)
                    steps = value.ToList();
            }
        }


        /// <summary>
        /// Adds a QuestStepData to steps collection if it doesnt allready exists.
        /// </summary>
        /// <param name="step"></param>
        public void AddStepData(QuestStepData step)
        {
            steps ??= new();

            if (step == null || steps.Contains(step)) return;

            steps.Add(step);
        }

    }
}