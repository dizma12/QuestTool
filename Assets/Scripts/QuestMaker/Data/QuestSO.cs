using QuestMaker.Data;
using QuestMaker.Data.Objectives;
using QuestMaker.Data.SpecialEvents;
using QuestMaker.Data.Steps;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


namespace QuestMaker.Data
{
    public class QuestSO : ScriptableObject
    {
        public string QuestID { get; set; } = string.Empty;
        public string QuestName
        {
            get => questName;
            set
            {
                if (questName.Equals(string.Empty))
                    questName = value;
                else return;
            }
        }
        public string QuestDescription { get; set; } = string.Empty;

        public PrerequisiteData Prerequisites { get => prerequisites; set => prerequisites = value; }
        public RewardData Rewards { get => rewards; set => rewards = value; }
        public IReadOnlyList<ObjectiveData> Objectives => objectives;

        [SerializeField, HideInInspector] string questName = string.Empty;

        [SerializeField]
        private PrerequisiteData prerequisites;

        [SerializeField]
        private RewardData rewards;

        [SerializeReference]
        private List<ObjectiveData> objectives;

        [SerializeField]
        private List<SpecialEventData> specialEvents = new();

        public void AddSpecialEvent(SpecialEventData data)
        {
            if(specialEvents.Contains(data)) return;
            specialEvents.Add(data);
        }

        public void AddPrerequisites(PrerequisiteData data)
        {
            prerequisites = data;
            Debug.Log($"[QuestSO] Added Prerequisites");
        }
        public void AddRewards(RewardData data)
        {
            rewards = data;
            Debug.Log("[QuestSO] Added Rewards!");
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
