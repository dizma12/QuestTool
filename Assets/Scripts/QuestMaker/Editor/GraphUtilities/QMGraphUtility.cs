using QuestMaker.Editor.Graph;
using UnityEngine;
using Unity.GraphToolkit.Editor;
using UnityEditor;

namespace QuestMaker.Editor.Utility
{
    internal static class QMGraphUtility
    {
        public static QMGraph LoadGraphFromAsset(QMGraphAssetFile asset)
        {
            string relativePath = AssetDatabase.GetAssetPath(asset);

            if (relativePath.Equals(string.Empty))
            {
                Debug.LogError($"Unable to Find asset path for asset {asset.name}");
            }

            QMGraph graph = GraphDatabase.LoadGraph<QMGraph>(relativePath);

            if (graph == null)
            {
                Debug.LogError($"Unable to Load Graph from asset {asset.name}");
            }
            Debug.Log($"Loaded Graph from asset {asset.name}");
            return graph;
        }
    }
}
