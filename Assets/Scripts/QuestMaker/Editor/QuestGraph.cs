using Unity.GraphToolkit.Editor;
using UnityEditor;
using System;


namespace QuestMaker.Editor.Graphs
{
    [Serializable]
    [Graph(AssetExtension)]
    public class QuestGraph : Graph
    {
        public const string AssetExtension = "qmkgraph";

        [MenuItem("Assets/Create/QuestToolkit/Quest Graph", false)]
        private static void CreateAssetFile()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<QuestGraph>();
        }
    }
}
