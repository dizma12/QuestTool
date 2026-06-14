using QuestMaker.Domain.Objectives;
using UnityEditor;
using UnityEngine;

namespace QuestMaker.Editor.CustomEditors
{
    [CustomPropertyDrawer(typeof(ObjectiveData))]
    public class ObjectiveDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var descProp = property.FindPropertyRelative("desc");
            var stepsProp = property.FindPropertyRelative("_steps");
            var eventsProp = property.FindPropertyRelative("_specialEvents");

            float y = position.y;

            // Description
            Rect rect = new(position.x, y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(rect, descProp);
            y += EditorGUIUtility.singleLineHeight + 2;

            // Steps
            float stepsHeight = EditorGUI.GetPropertyHeight(stepsProp, true);
            rect = new Rect(position.x, y, position.width, stepsHeight);
            EditorGUI.PropertyField(rect, stepsProp, true);
            y += stepsHeight + 2;

            // Only draw Special Events if not empty
            if (eventsProp != null && eventsProp.arraySize > 0)
            {
                float eventsHeight = EditorGUI.GetPropertyHeight(eventsProp, true);
                rect = new Rect(position.x, y, position.width, eventsHeight);
                EditorGUI.PropertyField(rect, eventsProp, true);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = 0;

            var descProp = property.FindPropertyRelative("desc");
            var stepsProp = property.FindPropertyRelative("_steps");
            var eventsProp = property.FindPropertyRelative("_specialEvents");

            height += EditorGUI.GetPropertyHeight(descProp, true) + 2;
            height += EditorGUI.GetPropertyHeight(stepsProp, true) + 2;

            if (eventsProp != null && eventsProp.arraySize > 0)
                height += EditorGUI.GetPropertyHeight(eventsProp, true) + 2;

            return height;
        }
    }
}
