using QuestMaker.Domain;
using QuestMaker.Domain.Events;
using QuestMaker.Domain.Objectives;
using QuestMaker.Domain.Quests;
using QuestMaker.Domain.SpecialEvents;
using QuestMaker.Domain.Steps;
using QuestMaker.Runtime.Events;
using QuestMaker.Runtime.Game;
using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable CS0618 // disables obsolete warning for QuestType.Hidden.
namespace QuestMaker.Runtime.Quests
{
    public class Quest
    {
        //Events
        public event Action<Quest> Changed;
        public event Action<Quest> Finished;

        //Public properties
        public bool IsFinished { get; protected set; } = false;
        public string ID { get; protected set; }
        public QuestStatus Status { get; protected set; } = QuestStatus.MISSING_REQUIRMENTS;
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
        public IReadOnlyCollection<QuestStep> CurrentSteps => _currentSteps;
        public IReadOnlyCollection<QuestStep> AllSteps
        {
            get
            {
                List<QuestStep> steps = new();
                foreach (var steparray in _objectives.Values)
                {
                    foreach (var step in steparray)
                        steps.Add(step);
                }
                return steps;
            }
        }
        public IReadOnlyCollection<ObjectiveData> Objectives => _questData.Objectives;
    

        //SO Data
        protected readonly QuestSO _questData;

        //Steps And Objectives
        protected readonly Dictionary<int, QuestStep[]> _objectives = new();
        protected QuestStep[] _currentSteps = new QuestStep[0];
        protected int _currentObjectiveIndex = 0;

        //Quest Info
        public readonly QuestType _qType = QuestType.Hidden;
        public readonly PrerequisiteData _prerequisites = null;
        public readonly RewardData _rewards = null;
        public readonly SpecialEventData _specialEvent = null;

        public Quest(QuestSO data, IQuestEventSource eventbus)
        {
            if (data == null)
                throw new ArgumentNullException("[Quest] QuestSO data cannot be null");

            _questData = data;
            _qType = data.QuestType;
            ID = data.ID;

            _prerequisites = data.Prerequisites;
            _rewards = data.Rewards;
            _specialEvent = data.SpecialEvent;

            CreateStepsForObjectives(eventbus);
        }

        private void CreateStepsForObjectives(IQuestEventSource eventbus)
        {
            for (int i = 0; i < _questData.Objectives.Count; i++)
            {
                ObjectiveData obj = _questData.Objectives[i];
                List<QuestStep> steps = new();

                foreach (var step in obj.Steps)
                {
                    steps.Add(step.CreateRuntimeStep(eventbus) as QuestStep);

                    ConsoleLogger.Log(this, $"Created QuestStep {step.GetType()} for objective {obj.ID}");
                }
                _objectives.Add(i, steps.ToArray());

            }
        }
        public void Start()
        {
            if(Status != QuestStatus.CAN_START) return; 

            if (_objectives == null || !_objectives.Any())
                throw new NullReferenceException($"[{ID}] objectives are null");

            SetQuestStatus(QuestStatus.IN_PROGRESS);

            _currentSteps = _objectives.First().Value;

            ActivateCurrentObjective();
            ConsoleLogger.Log(this, $"Quest with ID: {ID} started!");
        }
        //Public Methods
        public void NextObjective()
        {
            _currentObjectiveIndex++;

            if (_currentObjectiveIndex >= _objectives.Count - 1)
            {
                IsFinished = true;
                SetQuestStatus(QuestStatus.CAN_FINISH);
                ConsoleLogger.LogWarning(this, $"Change FireQuestCanFinish from quest to questmanager.");
                ReferenceManager.Instance.GetReference<GameEventManager>().RequestBus<QuestEventBus>().FireQuestCanFinish(this);
                Finished?.Invoke(this);
                return;
            }

            if (!_objectives.TryGetValue(_currentObjectiveIndex, out _currentSteps))
            {
                ConsoleLogger.LogWarning(this, $"Failed to load steps of objective at index= {_currentObjectiveIndex} on quest {ID}");
                return;
            }
            ActivateCurrentObjective();
        }

        public void SetQuestStatus(QuestStatus status)
        {
            if (Status != status)
                Status = status;
        }


        //Priavate Helpers
        private bool EvaluateObjective() => _objectives[_currentObjectiveIndex].All(step => step.IsComplete);

        private void OnStepChanged(QuestStep step) => Changed?.Invoke(this);
        private void OnStepFinished(QuestStep step)
        {

            DeactivateStep(step);

            if (EvaluateObjective())
            {
                NextObjective();
                return;
            }      
        }

        /// <summary>
        /// Activates all steps in the current objective.
        /// </summary>
        private void ActivateCurrentObjective()
        {
            foreach (QuestStep step in _currentSteps)
            {
                ActivateStep(step);
            }
        }


        /// <summary>
        /// Starts step and subs to Changed/Finished
        /// </summary>
        /// <param name="step"></param>
        private void ActivateStep(QuestStep step)
        {
            if (step.IsComplete) return;

            step.Start();
            step.Changed += OnStepChanged;
            step.Finished += OnStepFinished;

            ConsoleLogger.Log(this, $"{ID} step {step.GetType().Name} Activated!");
        }

        /// <summary>
        /// Unsubs from Changed/Finished
        /// </summary>
        /// <param name="step"></param>
        private void DeactivateStep(QuestStep step)
        {
            if (!step.IsComplete) return;

            step.Changed -= OnStepChanged;
            step.Finished -= OnStepFinished;
        }
    }
#pragma warning restore CS0618
}
