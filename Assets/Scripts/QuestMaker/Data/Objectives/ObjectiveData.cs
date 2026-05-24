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
            get => id;
            set
            {
                if (string.IsNullOrEmpty(id))
                    id = value;
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

        [SerializeField] private string id = string.Empty;
        [SerializeField] private string desc = string.Empty;

        [SerializeReference]
        private List<QuestStepData> steps = null;

        public IReadOnlyList<QuestStepData> Steps
        {
            get => steps;
            set
            {
                if ( (steps == null || !steps.Any()) && value != null)
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

            if(step == null || steps.Contains(step)) return;

            steps.Add(step);
        }

    }
}