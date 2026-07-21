using QuestMaker.Domain;
using QuestMaker.Editor.CustomEditors;
using UnityEditor;
using UnityEngine;

#pragma warning disable CS0618 // disables obsolete warning for QuestType.Hidden.
namespace QuestMaker.CustomEditor
{
    //***DISCLAIMER***
    // Most of this script is created by AI. (Do not punch me please)

    [UnityEditor.CustomEditor(typeof(QuestSO), true)] // true = also covers subclasses
    public class QuestSOEditor : UnityEditor.Editor
    {
        private SerializedProperty _qType;
        private SerializedProperty _objectives;
        private SerializedProperty _prerequisites;
        private SerializedProperty _rewards;
        private SerializedProperty _specialEvent;

        private void OnEnable()
        {
            _qType = serializedObject.FindProperty("_qType");
            _objectives = serializedObject.FindProperty("_objectives");
            _prerequisites = serializedObject.FindProperty("_prerequisites");
            _rewards = serializedObject.FindProperty("_rewards");
            _specialEvent = serializedObject.FindProperty("_specialEvent");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            //GUI.enabled = false; // full read-only
            using (new EditorGUI.DisabledScope(true))
            {
                // ── Identity ─────────────────────────────────────────────────
                DrawSectionHeader("Identity");
                EditorGUILayout.TextField("ID", (target as QuestSO).ID);
                EditorGUILayout.PropertyField(_qType, new GUIContent("Quest Type"));
                DrawSeparator();


                // ── Prerequisites ────────────────────────────────────────────
                if (_prerequisites != null && _prerequisites.managedReferenceValue != null)
                {
                    DrawSectionHeader("Prerequisites");
                    PrerequisiteDataDrawer.Draw(_prerequisites);
                    DrawSeparator();
                }

                // ── Rewards ──────────────────────────────────────────────────
                if (_rewards != null && _rewards.managedReferenceValue != null)
                {
                    DrawSectionHeader("Rewards");
                    RewardDataDrawer.Draw(_rewards);
                    DrawSeparator();
                }

                // ── Objectives ───────────────────────────────────────────────
                if (_objectives != null && _objectives.arraySize > 0)
                {
                    DrawSectionHeader("Goals");
                    EditorGUILayout.PropertyField(_objectives, new GUIContent("Objectives"), true);
                    DrawSeparator();
                }

                // ── Special Event ────────────────────────────────────────────
                if (_specialEvent != null && _specialEvent.managedReferenceValue != null)
                {
                    DrawSectionHeader("Special Event");
                    SpecialEventDataDrawer.Draw(_specialEvent);
                    DrawSeparator();
                }
            }
            //GUI.enabled = true;

            serializedObject.ApplyModifiedProperties();
        }

        // ─── Layout helpers ───────────────────────────────────────────────

        private static void DrawSectionHeader(string title)
        {
            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
        }

        private static void DrawSeparator()
        {
            EditorGUILayout.Space(4);
            Rect rect = EditorGUILayout.GetControlRect(false, 1);
            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 0.3f));
            EditorGUILayout.Space(4);
        }
    }
}



#pragma warning restore CS0618


