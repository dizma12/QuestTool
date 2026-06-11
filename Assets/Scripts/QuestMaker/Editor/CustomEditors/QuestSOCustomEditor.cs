using QuestMaker.Data;
using UnityEditor;
#pragma warning disable CS0618 // disables obsolete warning for QuestType.Hidden.
namespace QuestMaker.CustomEditor
{
    //***DISCLAIMER***
    // Most of this script is created by AI. (Do not punch me please)
    [UnityEditor.CustomEditor(typeof(QuestSO))]
    internal class QuestSOCustomEditor : UnityEditor.Editor
    {
        SerializedProperty _qName;
        SerializedProperty _qType;

        SerializedProperty _prerequisites;
        SerializedProperty _rewards;

        SerializedProperty _objectives;
        SerializedProperty _specialEvents;
        SerializedProperty _timeConstraint;

        // ---------------- FOLDOUT STATES ----------------
        private bool _showHeader = true;
        private bool _showPrerequisites = true;
        private bool _showRewards = true;
        private bool _showGoals = true;

        private void OnEnable()
        {
            _qName = serializedObject.FindProperty("_qName");
            _qType = serializedObject.FindProperty("_qType");

            _prerequisites = serializedObject.FindProperty("_prerequisites");
            _rewards = serializedObject.FindProperty("_rewards");

            _objectives = serializedObject.FindProperty("_objectives");
            _specialEvents = serializedObject.FindProperty("_specialEvent");
            _timeConstraint = serializedObject.FindProperty("_inGameTimeConstraint");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var quest = (QuestSO)target;

            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.LabelField(quest.QuestName, EditorStyles.boldLabel);
                EditorGUILayout.Space();

                DrawHeaderSection();
                EditorGUILayout.Space();

                DrawPrerequisitesSection();
                EditorGUILayout.Space();

                DrawRewardsSection();
                EditorGUILayout.Space();

                DrawGoalsSection();
            }

            serializedObject.ApplyModifiedProperties();
        }

        // ---------------- HEADER ----------------
        private void DrawHeaderSection()
        {
            _showHeader = EditorGUILayout.Foldout(_showHeader, "Quest Info", true);

            if (!_showHeader)
                return;

            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(_qName);


            EditorGUILayout.PropertyField(_qType);


            //Special Event
            var x = _specialEvents.FindPropertyRelative("_eventID");

            if (!string.IsNullOrEmpty(x.stringValue))
                EditorGUILayout.PropertyField(_specialEvents, true);


            EditorGUI.indentLevel--;
        }

        // ---------------- PREREQUISITES ----------------
        private void DrawPrerequisitesSection()
        {
            var level = _prerequisites.FindPropertyRelative("_level");
            var items = _prerequisites.FindPropertyRelative("_items");
            var quests = _prerequisites.FindPropertyRelative("_quests");
            var reps = _prerequisites.FindPropertyRelative("_reps");
            var timeConstraint = _prerequisites.FindPropertyRelative("_inGameTimeConstraint");

            bool hasData =
                level.intValue > 1 ||
                (items != null && items.arraySize > 0) ||
                (quests != null && quests.arraySize > 0) ||
                (reps != null && reps.arraySize > 0);

            if (!hasData)
                return;

            _showPrerequisites = EditorGUILayout.Foldout(_showPrerequisites, "Prerequisites", true);

            if (!_showPrerequisites)
                return;

            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(timeConstraint, true);

            if (level.intValue > 1)
                EditorGUILayout.PropertyField(level);

            if (items != null && items.arraySize > 0)
                EditorGUILayout.PropertyField(items, true);

            if (quests != null && quests.arraySize > 0)
                EditorGUILayout.PropertyField(quests, true);

            if (reps != null && reps.arraySize > 0)
                EditorGUILayout.PropertyField(reps, true);

            EditorGUI.indentLevel--;
        }

        // ---------------- REWARDS ----------------
        private void DrawRewardsSection()
        {
            var items = _rewards.FindPropertyRelative("_items");
            var exp = _rewards.FindPropertyRelative("_exp");
            var abilities = _rewards.FindPropertyRelative("_abilities");
            var reps = _rewards.FindPropertyRelative("_reps");



            bool hasData =
                (exp != null && exp.intValue > 0) ||
                (items != null && items.arraySize > 0) ||
                (abilities != null && abilities.arraySize > 0) ||
                (reps != null && reps.arraySize > 0);

            if (!hasData)
                return;

            _showRewards = EditorGUILayout.Foldout(_showRewards, "Rewards", true);

            if (!_showRewards)
                return;

            EditorGUI.indentLevel++;

            if (exp != null && exp.intValue > 0)
                EditorGUILayout.PropertyField(exp);

            if (items != null && items.arraySize > 0)
                EditorGUILayout.PropertyField(items, true);

            EditorGUI.indentLevel--;
        }

        // ---------------- GOALS ----------------
        private void DrawGoalsSection()
        {

            var steps = _objectives.FindPropertyRelative("_steps");
            var specialEvents = _objectives.FindPropertyRelative("_specialEvents");

            _showGoals = EditorGUILayout.Foldout(_showGoals, "Goals", true);

            if (!_showGoals)
                return;

            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(_objectives, true);

            //if(steps != null && steps.arraySize > 0)
            //    EditorGUILayout.PropertyField(steps, true);

            //foreach(var x in _objectives)
            //{
            //    EditorGUILayout.PropertyField(x., true);
            //}    

            //if (specialEvents != null && specialEvents.arraySize > 0)
            //    EditorGUILayout.PropertyField(specialEvents, true);


            EditorGUI.indentLevel--;
        }
    }


    #region Unused
    //[CustomEditor(typeof(QuestSO))]
    //internal class QuestSOCustomEditor : Editor
    //{
    //    public override void OnInspectorGUI()
    //    {
    //        var quest = (QuestSO)target;

    //        EditorGUILayout.LabelField(quest.QuestName, EditorStyles.boldLabel);
    //        EditorGUILayout.Space();

    //        using (new EditorGUI.DisabledScope(true))
    //        {
    //            DrawDefaultInspector();
    //        }
    //    }
    //}
    #endregion
#pragma warning restore CS0618
}

