using QuestMaker.Editor.Graph;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using QuestMaker.Editor.Utility;
using QuestMaker.Editor.Compiler;
namespace QuestMaker.Editor.Window
{
    public class QuestCompilerEditorWindow : EditorWindow
    {
        private QMGraphAssetFile assetFile = null;
        private QuestCompiler compiler = null;
        [MenuItem("QuestMaker/Compiler Window")]
        public static void ShowWindow()
        {

            var wnd = GetWindow<QuestCompilerEditorWindow>();
            wnd.titleContent = new GUIContent("Quest Compiler");

            // Lock the window size
            Vector2 windowSize = new(500, 280);
            wnd.minSize = windowSize;
            wnd.maxSize = windowSize;
        }
        private void CreateGUI()
        {
            #region Create Menu

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
                objectType = typeof(QMGraphAssetFile), // GraphAssetFile is a serialized object wrapper for Lookup in unity
                allowSceneObjects = false
            };
            root.Add(graphField);

            IntegerField nodeCountField = new("Node count")
            {
                isReadOnly = true

            };
            nodeCountField.SetEnabled(false);
            nodeCountField.value = 0;
            root.Add(nodeCountField);

            Button saveAssetBtn = new(() => { compiler.SaveQuestAsset(); }) { text = "Save as prefab" };
            {

                //saveAssetBtn.style.marginLeft = buttonOffset;
                //saveAssetBtn.style.marginRight = buttonOffset;
                saveAssetBtn.style.marginTop = 15;
                saveAssetBtn.style.flexDirection = FlexDirection.Row;
                saveAssetBtn.style.flexGrow = 0;
                saveAssetBtn.style.maxWidth = position.width * 1.3f;
                saveAssetBtn.SetEnabled(false);
                root.Add(saveAssetBtn);
            }
            graphField.RegisterValueChangedCallback(evt =>
            {
                assetFile = evt.newValue as QMGraphAssetFile;

                if (assetFile == null)
                {
                    Debug.Log("Asset is null");
                    nodeCountField.value = 0;
                    saveAssetBtn.SetEnabled(false);
                    return;
                }

                var graph = QMGraphUtility.LoadGraphFromAsset(assetFile);
                Debug.Log($"Loaded graph with name {graph.Name} and node count {graph.NodeCount} ");
                if (compiler == null)
                    compiler = new(graph);
                else compiler.SetGraph(graph);

                compiler.CompileQuestGraph();
                saveAssetBtn.SetEnabled(true);
                nodeCountField.value = graph.NodeCount;

            });
            #endregion

            #region Register Callbacks


            #endregion

        }
    }
}
