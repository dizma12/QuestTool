using QuestMaker.Editor.Graph;
using QuestMaker.Editor.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuestMaker.Editor.QuestMakerEditorWindow
{
    internal class CustoGraphEditorWindow : GraphViewToolWindow
    {
        protected override string ToolName => "The Name Buv";
        [MenuItem("QuestMaker/C Window")]
        public static void ShowWindow()
        {

            var wnd = CreateWindow<CustoGraphEditorWindow>();
            wnd.titleContent = new GUIContent("Quest Compiler");

            // Lock the window size
            Vector2 windowSize = new(500, 280);
            wnd.minSize = windowSize;
            wnd.maxSize = windowSize;
        }

        protected override void OnGraphViewChanged()
        {
            Debug.Log("g changed");
        }

        protected override void OnGraphViewChanging()
        {
            Debug.Log("g changing");
        }

        private void Awake()
        {
            Show();
        }
        private void CreateGUI()
        {


            VisualElement root = rootVisualElement;
            {
                root.style.flexDirection = FlexDirection.Column;
                root.style.flexGrow = 0;
                root.style.paddingBottom = 5;
                root.style.paddingTop = 5;
                root.style.paddingLeft = 2;
                root.style.paddingRight = 2;

       
            }
            Label title = new("Quest Compiler");
            {

                title.style.unityTextAlign = TextAnchor.MiddleCenter;
                title.style.fontSize = 14;
                title.style.unityFontStyleAndWeight = FontStyle.Bold;
                title.style.marginBottom = 10;

                root.Add(title);
            }


            ObjectField graphField = new("Graph")
            {
                objectType = typeof(QMGraph), // GraphAssetFile is a serialized object wrapper for Lookup in unity
                allowSceneObjects = false
            };
            root.Add(graphField);
        }
    }
}
