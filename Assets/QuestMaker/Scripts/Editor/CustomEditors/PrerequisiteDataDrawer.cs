using UnityEditor;
using UnityEngine;


namespace QuestMaker.Editor.CustomEditors
{
    //***DISCLAIMER*** THIS SCRIPT IS MADE WITH AI.(Do not punch me please)
    public static class PrerequisiteDataDrawer
    {
        /// <summary>
        /// Draws all fields of a PrerequisiteData [SerializeReference] property.
        /// Only draws fields that are non-null / non-empty / non-default.
        /// Assumes GUI.enabled is already set by the caller.
        /// </summary>
        public static void Draw(SerializedProperty prop)
        {
            if (prop == null || prop.managedReferenceValue == null) return;

            SerializedProperty level = prop.FindPropertyRelative("_level");
            SerializedProperty items = prop.FindPropertyRelative("_items");
            SerializedProperty reps = prop.FindPropertyRelative("_reps");
            SerializedProperty quests = prop.FindPropertyRelative("_quests");
            SerializedProperty timeConstr = prop.FindPropertyRelative("_inGameTimeConstraint");

            // Level: only show if above the default minimum of 1
            if (level != null && level.intValue > 1)
                EditorGUILayout.PropertyField(level, new GUIContent("Required Level"));

            DrawArrayIfNotEmpty(items, "Required Items");
            DrawArrayIfNotEmpty(reps, "Required Reputation");
            DrawArrayIfNotEmpty(quests, "Required Quests");

            // InGameTimeline is a struct — draw it if the property exists
            if (timeConstr != null)
                EditorGUILayout.PropertyField(timeConstr, new GUIContent("Time Constraint"), true);
        }

        private static void DrawArrayIfNotEmpty(SerializedProperty prop, string label)
        {
            if (prop == null || prop.arraySize <= 0) return;
            EditorGUILayout.PropertyField(prop, new GUIContent(label), true);
        }
    }
}
