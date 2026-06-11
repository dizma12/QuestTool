using QuestMaker.Data;
using QuestMaker.Data.Objectives;
using QuestMaker.Data.SpecialEvents;
using QuestMaker.Data.Steps;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable CS0618 // disables obsolete warning for QuestType.Hidden.
namespace QuestMaker.Data
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
        public string QuestID => _qName;
        public virtual string QuestName
        {
            get => _qName;
            set
            {
                if (string.IsNullOrEmpty(_qName) && !string.IsNullOrEmpty(value))
                    _qName = value;
            }
        }
        public string Description { get; set; } = string.Empty;

        public PrerequisiteData Prerequisites { get => _prerequisites; set => _prerequisites = value; }

        public RewardData Rewards { get => _rewards; set => _rewards = value; }
        public IReadOnlyList<ObjectiveData> Objectives => _objectives;
        public SpecialEventData SpecialEvent
        {
            get => _specialEvent;

            set
            {
                //if (_specialEvent.Equals(default) && !value.Equals(default)) 
                    _specialEvent = value;
            }
        }
        [SerializeField, HideInInspector]
        protected string _qName = string.Empty;

        //[Header("Info")]


        [SerializeField]
        protected QuestType _qType = QuestType.Hidden;

        [SerializeField]
        protected PrerequisiteData _prerequisites;

        [SerializeField]
        protected RewardData _rewards;

        //[Header("Goals")]
        [SerializeReference]
        protected List<ObjectiveData> _objectives;

        [SerializeField]
        protected SpecialEventData _specialEvent;


        public void AddObjective(ObjectiveData obj)
        {
            _objectives ??= new();

            if (obj == null || _objectives.Contains(obj)) return;

            _objectives.Add(obj);
            Debug.Log($"[QuestSO] Objective {obj.ID} added!");
        }
    }
#pragma warning restore CS0618
}

