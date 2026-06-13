using QuestMaker.Data;
using QuestMaker.Data.Objectives;
using QuestMaker.Data.SpecialEvents;
using QuestMaker.Runtime.StepsAndObjectives;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#pragma warning disable CS0618 // disables obsolete warning for QuestType.Hidden.
namespace QuestMaker.Core.Quests
{
    public class Quest
    {
        public bool IsFinished { get; protected set; } = false;
        public string ID { get; protected set; }
        public QuestStatus Status { get; protected set; } = QuestStatus.MISSING_REQUIRMENTS;

        protected readonly QuestSO _questData;
        protected readonly Dictionary<int, QuestStep[]> _objectives = new();

        protected int _currentObjectiveIndex = 0;
        protected int _currentStepIndex = 0;


        protected readonly QuestType _qType = QuestType.Hidden;

        // Initialize structs to new() or default so we can do equality check with default (ex. _prerequisites.Equals(default(PrerequisiteData)))
        // *** IMPORTANT *** default without specifing struct type converts to object default always returns false
        // ALWAYS USE default(structType).

        protected readonly PrerequisiteData _prerequisites = default;

        protected readonly RewardData _rewards = default;

        protected readonly SpecialEventData _specialEvent = default;

        public ObjectiveData CurrentObjective
        {
            get
            {
                if (_currentObjectiveIndex >= _questData.Objectives.Count)
                    return _questData.Objectives[^1];
                else
                    return _questData.Objectives[_currentObjectiveIndex];
            }
        }
        public QuestStep CurrentStep
        {
            get
            {
                if (_currentStepIndex >= _objectives[_currentObjectiveIndex].Length)
                    return _objectives.Last().Value[^1];
                else
                    return _objectives[_currentObjectiveIndex][_currentStepIndex];
            }
        }
        public QuestStep[] CurrentSteps
        {
            get
            {
                if (_currentStepIndex >= _objectives[_currentObjectiveIndex].Length)
                    return _objectives.Last().Value;
                else
                    return _objectives[_currentObjectiveIndex];
            }
        }

        public Quest(QuestSO data)
        {
            if (data == null)
                throw new ArgumentNullException("[Quest] QuestSO data cannot be null");

            _questData = data;
            _qType = data.QuestType;
            ID = data.ID;

            _prerequisites = data.Prerequisites;
            _rewards = data.Rewards;
            _specialEvent = data.SpecialEvent;

            CreateStepsForObjectives();
        }

        private void CreateStepsForObjectives()
        {
            for (int i = 0; i < _questData.Objectives.Count; i++)
            {
                ObjectiveData obj = _questData.Objectives[i];
                List<QuestStep> steps = new();

                foreach (var step in obj.Steps)
                {
                    steps.Add(RuntimeStepCreator.CreateQuestStep(step));

                    Debug.Log($"Created QuestStep {step.GetType()} for objective {obj.ID}");
                }
                _objectives.Add(i, steps.ToArray());
            }
        }

        public void NextObjective()
        {
            _currentObjectiveIndex++;

            if (_currentObjectiveIndex >= _objectives.Count)
                IsFinished = true;
        }

        public void NextStep()
        {
            _currentStepIndex++;

            if (_currentStepIndex >= _objectives[_currentObjectiveIndex].Length)
            {
                _currentStepIndex = 0;
                NextObjective();
            }
        }

        public void SetQuestStatus(QuestStatus status)
        {
            if (Status != status)
                Status = status;
        }
    }
#pragma warning restore CS0618
}
