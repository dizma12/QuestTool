using Unity.GraphToolkit.Editor;
using UnityEditor;
using System;
using UnityEngine;


namespace QuestMaker.Editor.Graph
{
    [Graph(AssetExtension , GraphOptions.SupportsSubgraphs)]
    [Serializable]
    public class QMGraph : Unity.GraphToolkit.Editor.Graph
    {
        public const string AssetExtension = "qmgraph";

        [MenuItem("Assets/Create/QuestToolkit/Quest Graph", false)]
        private static void CreateGraphFile()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<QMGraph>();
        }
    }
}
