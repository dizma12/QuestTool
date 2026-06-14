using UnityEditor;
using UnityEngine;

namespace QuestMaker.Domain.Helpers
{
    // I would rly love to put this on the Editor Assembly but i get a fcking cycling dependency error.
    public class ReadOnlyInspectorAttribute : PropertyAttribute { }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(ReadOnlyInspectorAttribute))]
    public class ReadOnlyInspectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = true;
        }
    }
#endif
}
