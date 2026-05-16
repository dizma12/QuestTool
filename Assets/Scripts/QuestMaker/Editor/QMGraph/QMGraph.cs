using Unity.GraphToolkit.Editor;
using UnityEditor;
using System;
using UnityEngine;
using System.Linq;
using QuestMaker.Editor.Nodes;
using System.Collections.Generic;


namespace QuestMaker.Editor.Graph
{
    [Graph(AssetExtension, GraphOptions.SupportsSubgraphs)]
    [Serializable]
    public class QMGraph : Unity.GraphToolkit.Editor.Graph
    {
        private static int changesCount = 0;
        public const string AssetExtension = "qmgraph";

        private Dictionary<Type, QMBaseContextNode> savedNodes = null;
        GraphLogger log = null;
        [MenuItem("Assets/Create/QuestToolkit/Quest Graph", false)]
        private static void CreateGraphFile()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<QMGraph>();
        }
        public override void OnGraphChanged(GraphLogger logger)
        {
            changesCount++;
            logger.Log(changesCount);

            log ??= logger;
            savedNodes ??= new();
            base.OnGraphChanged(logger);

            IEnumerable<QMBaseContextNode> currentNodes = GetCurrentContextNodes();
            var x = FilterDuplicates(currentNodes.ToHashSet(), out List<QMBaseContextNode> duplicates);
            UpdateContextNodes(x);
            if (duplicates.Count > 0)
            {
                foreach (var d in duplicates)
                {
                    logger.LogWarning($"Can not have  multiple instances of {d.GetType()}", d);
                }
            }

            //CheckDuplicates(logger, currentNodes);
        }
        private List<QMBaseContextNode> FilterDuplicates(HashSet<QMBaseContextNode> nodesToCheck, out List<QMBaseContextNode> duplicates)
        {                                               //we use hashset for faster lookups.
            List<QMBaseContextNode> results = new();
            duplicates = new();

            foreach (QMBaseContextNode node in nodesToCheck)
            {
                Type type = node.GetType();

                if (savedNodes.TryGetValue(type, out QMBaseContextNode existingNode))
                {
                    bool existingNodeStillExists =
                        nodesToCheck.Contains(existingNode);

                    if (node == existingNode || !existingNodeStillExists)
                        results.Add(node);

                    else
                        duplicates.Add(node);

                }
                else
                    results.Add(node); 
            }
            return results;
        }

        private IEnumerable<QMBaseContextNode> GetCurrentContextNodes()
        {
            return GetNodes()
                .OfType<QMBaseContextNode>()
                .Where(n => n.AllowMultipleContextNodesOfSameType == false);
        }

        private void UpdateContextNodes(IEnumerable<QMBaseContextNode> newNodes)
        {
            if (savedNodes == null) savedNodes = new();
            else savedNodes.Clear();

            foreach (QMBaseContextNode node in newNodes)
            {
                savedNodes.Add(node.GetType(), node);
            }
        }
    }

}
