using QuestMaker.Editor.Graph;
using UnityEngine;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using System;
using System.Linq;
using Unity.Properties;

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

            QMGraph graph = GraphDatabase.LoadGraph<QMGraph>(relativePath)
                          ?? throw new InvalidPathException(relativePath);  
            return graph;
        }

        public static Type GetTypeFromGenericInterface(Type lookUpType, Type interfaceToLookfor)
        {
            return lookUpType
                .GetInterfaces()
                .Where(i => i.IsGenericType)
                .Where(i => i.GetGenericTypeDefinition() == interfaceToLookfor)
                .Select(i => i.GetGenericArguments().First())
                .FirstOrDefault();
        }
    }
}
