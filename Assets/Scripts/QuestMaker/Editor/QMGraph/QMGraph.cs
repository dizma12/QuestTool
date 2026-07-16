using Unity.GraphToolkit.Editor;
using UnityEditor;
using System;
using System.Linq;
using QuestMaker.Editor.Nodes;
using System.Collections.Generic;
using QuestMaker.Domain;


namespace QuestMaker.Editor.Graph
{
    [Graph(AssetExtension, GraphOptions.SupportsSubgraphs)]
    [Serializable]
    public class QMGraph : Unity.GraphToolkit.Editor.Graph
    {
        private static int changesCount = 0;
        public const string AssetExtension = "qmgraph";

        private Dictionary<Type, QMBaseContextNode> savedNodes = null;

        [MenuItem("Assets/Create/QuestMaker/Quest Graph", false)]
        private static void CreateGraphFile()
        {
            GraphDatabase.PromptInProjectBrowserToCreateNewAsset<QMGraph>();
        }
        public override void OnGraphChanged(GraphLogger logger)
        {
            changesCount++;
            logger.Log(changesCount);

            savedNodes ??= new();
            base.OnGraphChanged(logger);

            //locate and validate Starting Node
            QMStartingNode start = GetStartingNode(logger);

            if (start == null) return;

            if (!ValidateStartingNode(logger, start)) return;


            HashSet<QMBaseContextNode> currentNodes = GetCurrentContextNodes().ToHashSet();
            var uniqueNodes = FilterDuplicates(currentNodes, out HashSet<QMBaseContextNode> duplicates);
            UpdateContextNodes(uniqueNodes);

            if (duplicates.Count > 0)
            {
                foreach (var d in duplicates)
                {
                    logger.LogWarning($"Can not have  multiple instances of {d.GetType().Name}", d);
                }
            }

            //CheckDuplicates(logger, currentNodes);
        }
        private HashSet<QMBaseContextNode> FilterDuplicates(HashSet<QMBaseContextNode> nodesToCheck, out HashSet<QMBaseContextNode> duplicates)
        {                                               //we use hashset for faster lookups.
            HashSet<QMBaseContextNode> results = new();
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
        private QMStartingNode GetStartingNode(GraphLogger log)
        {

            ConsoleLogger.Log(this, "Trying To Locate Starting Node");
            IEnumerable<QMStartingNode> start = GetNodes().OfType<QMStartingNode>();
            int count = start.Count();
            if (count <= 0)
            {
                log.LogWarning($"Graph requires a Starting Node!");
                return null;
            }
            else if (count > 1)
            {
                foreach (QMStartingNode node in start)
                    log.LogWarning($"Graph CAN NOT have more that 1 starting node!", node);

                return null;
            }
            return start.Single();
        }

        private bool ValidateStartingNode(GraphLogger log, QMStartingNode start)
        {
            if (start == null) return false;
  
            IPort startPort = start.GetOutputPortByName(QMStartingNode.OBJECTIVE_FLOW_PORT);
            List<IPort> ports = new();
            startPort.GetConnectedPorts(ports);

            if (ports.Count <= 0)
            {
                log.LogWarning("Starting Node Has No Objectives wired" , start);
                return false;
            }
            else if (ports.Count > 1)
            {
                foreach (IPort inputPort in ports)
                {
                    log.LogWarning("Starting Node Objective port CAN NOT have multiple objectives wired." +
                        "To chain Objectives use the NextObjective Port of Objective Node", inputPort.GetNode());

                    ConsoleLogger.Log(this, "Starting Node Objective port CAN NOT have multiple objectives wired." +
                        $"To chain Objectives use the NextObjective Port of Objective Node {inputPort.GetNode()}");
                }
            }
            return true;
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
