using QuestMaker.Data;
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

        [SerializeField, HideInInspector] string questName = string.Empty;

        public PrerequisiteData Prerequisites { get => prerequisites; set => prerequisites = value; }
        public RewardData Rewards { get => rewards; set => rewards = value; }

        [SerializeField]
        private PrerequisiteData prerequisites;

        [SerializeField]
        private RewardData rewards;

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
