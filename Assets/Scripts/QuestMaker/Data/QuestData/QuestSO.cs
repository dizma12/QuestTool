using QuestMaker.Data;
using QuestMaker.Data.Objectives;
using QuestMaker.Data.SpecialEvents;
using QuestMaker.Data.Steps;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
                if (string.IsNullOrEmpty(_qName))
                    _qName = value;
                else return;
            }
        }
        public string Description { get; set; } = string.Empty;

        public PrerequisiteData Prerequisites { get => _prerequisites; set => _prerequisites = value; }

        public RewardData Rewards { get => _rewards; set => _rewards = value; }
        public IReadOnlyList<ObjectiveData> Objectives => _objectives;

        [SerializeField, HideInInspector] protected string _qName = string.Empty;

        [Header("Info")]
        [SerializeField]
        protected QuestType _qType = QuestType.Hidden;

        [SerializeField]
        protected PrerequisiteData _prerequisites;

        [SerializeField]
        protected RewardData _rewards;

        [Header("Goals")]
        [SerializeReference]
        protected List<ObjectiveData> _objectives;

        [SerializeField]
        protected List<SpecialEventData> _specialEvents = new();


        public void AddSpecialEvent(SpecialEventData data)
        {
            if (_specialEvents.Contains(data)) return;
            _specialEvents.Add(data);
        }

        public void AddObjective(ObjectiveData obj)
        {
            _objectives ??= new();

            if (obj == null || _objectives.Contains(obj)) return;

            _objectives.Add(obj);
            Debug.Log($"[QuestSO] Objective {obj.ID} added!");
        }
    }

    [CustomEditor(typeof(QuestSO))]
    internal class QuestSOCustomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var quest = (QuestSO)target;

            EditorGUILayout.LabelField(quest.QuestName, EditorStyles.boldLabel);
            EditorGUILayout.Space();

            using (new EditorGUI.DisabledScope(true))
            {
                DrawDefaultInspector();
            }
        }
    }
}
