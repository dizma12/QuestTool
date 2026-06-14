using UnityEditor;
using UnityEngine;

namespace QuestMaker.Editor.CustomEditors
{
    //***DISCLAIMER*** THIS SCRIPT IS MADE WITH AI.
    public static class RewardDataDrawer
    {
        /// <summary>
        /// Draws all fields of a RewardData [SerializeReference] property.
        /// Only draws fields that are non-null / non-empty.
        /// Assumes GUI.enabled is already set by the caller.
        /// </summary>
        public static void Draw(SerializedProperty prop)
        {
            if (prop == null || prop.managedReferenceValue == null) return;

            SerializedProperty exp = prop.FindPropertyRelative("_exp");
            SerializedProperty items = prop.FindPropertyRelative("_items");
            SerializedProperty abilities = prop.FindPropertyRelative("_abilities");
            SerializedProperty reps = prop.FindPropertyRelative("_reps");

            if (exp != null && exp.intValue != 0)
                EditorGUILayout.PropertyField(exp, new GUIContent("Experience"));

            DrawArrayIfNotEmpty(items, "Items");
            DrawArrayIfNotEmpty(abilities, "Abilities");
            DrawArrayIfNotEmpty(reps, "Reputation");
        }

        private static void DrawArrayIfNotEmpty(SerializedProperty prop, string label)
        {
            if (prop == null || prop.arraySize == 0) return;
            EditorGUILayout.PropertyField(prop, new GUIContent(label), true);
        }
    }
}

