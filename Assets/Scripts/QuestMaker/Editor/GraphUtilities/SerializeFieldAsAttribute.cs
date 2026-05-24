using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class SerializeFieldAsAttribute : PropertyAttribute
{
    public string alias;
    public SerializeFieldAsAttribute(string alias)
    {
        this.alias = alias;
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(SerializeFieldAsAttribute))]
    public class ThisPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent alias)
        {
            var propertyAttribute = this.attribute as SerializeFieldAsAttribute;
            alias.text = propertyAttribute.alias;
            EditorGUI.PropertyField(position, property, alias);
        }
    }

#endif
}