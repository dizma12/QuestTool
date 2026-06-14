using QuestMaker.Domain.SpecialEvents;
using QuestMaker.Domain.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QuestMaker.Domain.Objectives
{
    [Serializable]
    public class ObjectiveData
    {

        [SerializeField, HideInInspector] private string id = string.Empty;
        [SerializeField] private string desc = string.Empty;

        [SerializeReference]
        private List<QuestStepData> _steps = null;

        [SerializeField]
        private List<SpecialEventData> _specialEvents = null;
        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(id))
                {
                    if (_steps != null && _steps.Count > 0)
                    {
                        var x = _steps.Select(s => s.GetType().Name.Replace("StepData", string.Empty));
                        id = string.Join("_", x) + $"_{_steps.Count}";
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


        public IReadOnlyList<QuestStepData> Steps
        {
            get => _steps;
            set
            {
                if ((_steps == null || !_steps.Any()) && value != null)
                    _steps = value.ToList();
            }
        }

        public IReadOnlyList<SpecialEventData> SpecialEvents
        {
            get => _specialEvents;
            set
            {
                if ((_specialEvents == null || !_specialEvents.Any()) && value != null)
                    _specialEvents = value.ToList();
            }
        }

        /// <summary>
        /// Adds a QuestStepData to steps collection if it doesnt allready exists.
        /// </summary>
        /// <param name="step"></param>
        public void AddStepData(QuestStepData step)
        {
            _steps ??= new();

            if (step == null || _steps.Contains(step)) return;

            _steps.Add(step);
        }

        /// <summary>
        /// Adds a QuestStepData to steps collection if it doesnt allready exists.
        /// </summary>
        /// <param name="step"></param>
        public void AddSpecialEvent(SpecialEventData eventdata)
        {
            _specialEvents ??= new();

            if (eventdata.Equals(default)) return;

            _specialEvents.Add(eventdata);
        }

    }
}
