using UnityEditor;
using UnityEngine;

namespace QuestMaker.Editor.CustomEditors
{
    //***DISCLAIMER*** THIS SCRIPT IS MADE WITH AI.(Do not punch me please)
    public static class SpecialEventDataDrawer
    {
        /// <summary>
        /// Draws all fields of a SpecialEventData [SerializeReference] property.
        /// Only draws fields that are non-null / non-empty.
        /// Assumes GUI.enabled is already set by the caller.
        /// </summary>
        public static void Draw(SerializedProperty prop)
        {
            if (prop == null || prop.managedReferenceValue == null) return;

            SerializedProperty trigger = prop.FindPropertyRelative("_trigger");
            SerializedProperty eventID = prop.FindPropertyRelative("_eventID");

            if (trigger != null)
                EditorGUILayout.PropertyField(trigger, new GUIContent("Trigger"));

            if (eventID != null && !string.IsNullOrEmpty(eventID.stringValue))
                EditorGUILayout.PropertyField(eventID, new GUIContent("Event ID"));
        }
    }
}