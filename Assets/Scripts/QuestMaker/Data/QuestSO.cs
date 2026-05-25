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
        public string QuestID { get => name; }
        public virtual string QuestName
        {
            get => questName;
            set
            {
                if (string.IsNullOrEmpty(questName))
                    questName = value;
                else return;
            }
        }
        public string QuestDescription { get; set; } = string.Empty;

        public PrerequisiteData Prerequisites { get => prerequisites; set => prerequisites = value; }
        public RewardData Rewards { get => rewards; set => rewards = value; }
        public IReadOnlyList<ObjectiveData> Objectives => objectives;

        [SerializeField, HideInInspector] protected string questName = string.Empty;

        [SerializeField]
        protected PrerequisiteData prerequisites;

        [SerializeField]
        protected RewardData rewards;

        [SerializeReference]
        protected List<ObjectiveData> objectives;

        [SerializeField]
        protected List<SpecialEventData> specialEvents = new();

        public void AddSpecialEvent(SpecialEventData data)
        {
            if(specialEvents.Contains(data)) return;
            specialEvents.Add(data);
        }

        public void AddObjective(ObjectiveData obj )
        {
            objectives ??= new();

            if (obj == null || objectives.Contains(obj)) return;

            objectives.Add(obj);
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
