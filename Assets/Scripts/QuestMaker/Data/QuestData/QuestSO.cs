using QuestMaker.Domain.Helpers;
using QuestMaker.Domain.Objectives;
using QuestMaker.Domain.Quests;
using QuestMaker.Domain.SpecialEvents;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable CS0618 // disables obsolete warning for QuestType.Hidden.

namespace QuestMaker.Domain
{
    public class QuestSO : ScriptableObject
    {
        public QuestType QuestType
        {
            get => _qType;
            set
            {

                if (_qType == QuestType.Hidden && value != QuestType.Hidden)
                    _qType = value;

            }
        }
        public virtual string ID => name;

        public string Description { get; set; } = string.Empty;
        public IReadOnlyList<ObjectiveData> Objectives => _objectives;

        public PrerequisiteData Prerequisites
        {
            get => _prerequisites;
            set
            {
                if (value == null || _prerequisites == value) return;
                _prerequisites = value;
            }
        }
        public string HandInGiverGuid => _handInGiverGuid;
        public string TurnInGiverGuid => _turnInGiverGuid;
        public RewardData Rewards
        {
            get => _rewards;
            set
            {
                if (value == null || _rewards == value) return;
                _rewards = value;
            }
        }
        public SpecialEventData SpecialEvent
        {
            get => _specialEvent;

            set
            {
                _specialEvent = value;
            }
        }
        [SerializeReference]
        protected List<ObjectiveData> _objectives;


        [SerializeField]
        protected QuestType _qType = QuestType.Hidden;


        [SerializeReference]
        protected PrerequisiteData _prerequisites = null;

        [SerializeReference]
        protected RewardData _rewards = null;

        [SerializeReference]
        protected SpecialEventData _specialEvent = null;

        [SerializeField, ReadOnlyInspector]
        protected string _handInGiverGuid = string.Empty;

        [SerializeField, ReadOnlyInspector]
        protected string _turnInGiverGuid = string.Empty;


        public void AddObjective(ObjectiveData obj)
        {
            _objectives ??= new();

            if (obj == null || _objectives.Contains(obj)) return;

            _objectives.Add(obj);
            ConsoleLogger.Log(this, $"Objective {obj.ID} added!");
        }

#if UNITY_EDITOR
        public void Rename(string newName)
        {
            if (string.IsNullOrEmpty(newName)) return;
            name = newName;
        }

        public void SetHandInGiver(string guid)
        {
            if (!string.IsNullOrEmpty(guid)) _handInGiverGuid = guid;
        }

        public void SetTurnInGiver(string guid)
        {
            if (!string.IsNullOrEmpty(guid)) _turnInGiverGuid = guid;
        }
#endif
    }
#pragma warning restore CS0618
}

