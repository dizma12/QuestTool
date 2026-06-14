using QuestMaker.Editor.Graph;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using QuestMaker.Editor.Utility;
using QuestMaker.Editor.Compiler;
using System.Linq;
using QuestMaker.Domain;
namespace QuestMaker.Editor.Window
{
    public class QuestCompilerEditorWindow : EditorWindow
    {
        private QMGraphAssetFile assetFile = null;
        private QuestCompiler compiler = null;
        ObjectField graphField = null;
        IntegerField nodeCountField = null;
        VisualElement Root = null;

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
            DrawWindow();
        }

        private void DrawWindow()
        {
            DrawMenuHeader();
            DrawMenu(Root);
            //DrawPrerequisites(Root);
        }
        private void DrawMenuHeader()
        {
            if (Root == null)
            {
                Root = rootVisualElement;
                {
                    Root.style.flexDirection = FlexDirection.Column;
                    Root.style.flexGrow = 0;
                    Root.style.paddingBottom = 5;
                    Root.style.paddingTop = 5;
                    Root.style.paddingLeft = 2;
                    Root.style.paddingRight = 2;
                }
            }

            Label title = new("Quest Compiler");
            {

                title.style.unityTextAlign = TextAnchor.MiddleCenter;
                title.style.fontSize = 14;
                title.style.unityFontStyleAndWeight = FontStyle.Bold;
                title.style.marginBottom = 10;

                Root.Add(title);
            }
        }

        private void DrawMenu(VisualElement root)
        {
            VisualElement body = new();

            graphField = new("Graph")
            {
                objectType = typeof(QMGraphAssetFile), // GraphAssetFile is a serialized object wrapper for Lookup in unity
                allowSceneObjects = false
            };
            body.Add(graphField);

            nodeCountField = new("Node count")
            {
                isReadOnly = true

            };
            nodeCountField.SetEnabled(false);
            nodeCountField.value = 0;

            body.Add(nodeCountField);

            Button saveAssetBtn = new(() => { compiler.SaveQuestAsset(); ResetGraph(); }) { text = "Save as prefab" };
            {

                //saveAssetBtn.style.marginLeft = buttonOffset;
                //saveAssetBtn.style.marginRight = buttonOffset;
                saveAssetBtn.style.marginTop = 15;
                saveAssetBtn.style.marginLeft = Screen.width * 0.15f;
                saveAssetBtn.style.marginRight = Screen.width * 0.15f;
                saveAssetBtn.style.flexDirection = FlexDirection.Row;
                saveAssetBtn.style.flexGrow = 0;
                saveAssetBtn.style.width = Screen.width * ( (1 - 0.15f * 2f) - 0.1f);//* 1.3f;
                saveAssetBtn.style.maxWidth = saveAssetBtn.style.width;
                saveAssetBtn.SetEnabled(false);
                body.Add(saveAssetBtn);
            }

            //Register Callbacks
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
                
                saveAssetBtn.SetEnabled(compiler.Quest != null);

                DrawPrerequisites(root);
                nodeCountField.value = graph.NodeCount;

            });
            Root.Add(body);
        }

        private void DrawPrerequisites(VisualElement root)
        {
            //VisualElement prerequisites = new();
            //Label title = new("Prerequisites");
            //{

            //    title.style.unityTextAlign = TextAnchor.MiddleCenter;
            //    title.style.fontSize = 10;
            //    title.style.unityFontStyleAndWeight = FontStyle.Bold;
            //    title.style.marginTop = 10;
            //    title.style.marginBottom = 10;

            //    prerequisites.Add(title);
            //}

            //IntegerField Level = new()
            //{
            //    isReadOnly = true,
            //    value = compiler.Quest.Prerequisites.Level

            //};
            ////Level.SetEnabled(false);

            //prerequisites.Add(Level);
            //Label questLabel = new("Prerequisites");
            //{


            //    questLabel.style.fontSize = 10;
            //    questLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            //    questLabel.style.marginBottom = 10;

            //    prerequisites.Add(questLabel);
            //}
            //if (compiler.Quest.Prerequisites.Quests.Any())
            //{
            //    var Qprepreq = compiler.Quest.Prerequisites.Quests;
            //    for (int i = 0; i < Qprepreq.Count; i++)
            //    {
            //        ObjectField field = new($"Quest Requirment [{i}]")
            //        {
            //            objectType = typeof(QuestSO),
            //            allowSceneObjects = false,
            //            value = Qprepreq[i]
            //        };
            //        field.SetEnabled(false);
            //        prerequisites.Add(field);
            //    }
            //}

            //if (compiler.Quest.Prerequisites.Items.Any())
            //{
            //    var Itemprepreq = compiler.Quest.Prerequisites.Items;
            //    for (int i = 0; i < Itemprepreq.Count; i++)
            //    {
            //        ObjectField field = new($"Item Requirment [{i}]")
            //        {
            //            objectType = typeof(Item),
            //            allowSceneObjects = false,
            //            value = Itemprepreq[i].Item
            //        };
            //        field.SetEnabled(false);
            //        prerequisites.Add(field);
            //    }
            //}

            //root.Add(prerequisites);
        }

        private void ResetGraph()
        {
            assetFile = null;
            graphField = null;
            nodeCountField = null;
            compiler.ResetGraph();
            Root.Clear();
            DrawWindow();
        }
    }
}
