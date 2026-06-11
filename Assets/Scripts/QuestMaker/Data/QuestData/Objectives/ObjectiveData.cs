using QuestMaker.Data.SpecialEvents;
using QuestMaker.Data.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace QuestMaker.Data.Objectives
{
    [Serializable]
    public class ObjectiveData
    {

        [SerializeField, HideInInspector] private string id = string.Empty;
        [SerializeField] private string desc = string.Empty;

        [SerializeReference]
        private List<QuestStepData> _steps = null;

        [SerializeField]
        private List<SpecialEventData> _specialEvents = null;
        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(id))
                {
                    if (_steps != null && _steps.Count > 0)
                    {
                        var x = _steps.Select(s => s.StepType.ToString());
                        id = string.Join("_", x) + $"_{_steps.Count}";
                    }
                }
                return id;
            }

        }
        public string Description
        {
            get => desc;
            set
            {
                if (string.IsNullOrEmpty(desc))
                    desc = value;
            }
        }


        public IReadOnlyList<QuestStepData> Steps
        {
            get => _steps;
            set
            {
                if ((_steps == null || !_steps.Any()) && value != null)
                    _steps = value.ToList();
            }
        }

        public IReadOnlyList<SpecialEventData> SpecialEvents
        {
            get => _specialEvents;
            set
            {
                if ((_specialEvents == null || !_specialEvents.Any()) && value != null)
                    _specialEvents = value.ToList();
            }
        }

        /// <summary>
        /// Adds a QuestStepData to steps collection if it doesnt allready exists.
        /// </summary>
        /// <param name="step"></param>
        public void AddStepData(QuestStepData step)
        {
            _steps ??= new();

            if (step == null || _steps.Contains(step)) return;

            _steps.Add(step);
        }

        /// <summary>
        /// Adds a QuestStepData to steps collection if it doesnt allready exists.
        /// </summary>
        /// <param name="step"></param>
        public void AddSpecialEvent(SpecialEventData eventdata)
        {
            _specialEvents ??= new();

            if (eventdata.Equals(default)) return;

            _specialEvents.Add(eventdata);
        }

    }

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