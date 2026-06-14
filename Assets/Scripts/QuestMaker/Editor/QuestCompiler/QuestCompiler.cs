using QuestMaker.Domain;
using QuestMaker.Editor.CompilationModules;
using QuestMaker.Editor.Graph;
using QuestMaker.Editor.Nodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEngine;

namespace QuestMaker.Editor.Compiler
{

    public class QuestCompiler
    {
        private QMGraph graph = null;
        private readonly ModuleBuilderRegistry reg = null;
        private readonly HashSet<Type> proccessedNodes = null;
        public QuestSO Quest { get; private set; } = null;
        public QuestCompiler(QMGraph graph)
        {
            this.graph = graph;
            reg = new();
            proccessedNodes = new();
        }

        private QMStartingNode startingNode = null;
        public void CompileQuestGraph()
        {
            IEnumerable<INode> graphNodes = graph.GetNodes();

            if (!graphNodes.Any()) return;

            INode startNode = LocateStartingNode(graphNodes);

            startingNode.ProcessNode(reg);

            ProcessStartingNodeByPort(startNode, QMStartingNode.CONTEXT_FLOW_PORT);
            ProcessStartingNodeByPort(startNode, QMStartingNode.OBJECTIVE_FLOW_PORT);

            Quest = BuildQuest();

            //Debug.Log($"The name of the quest is: {Quest.QuestName}");
            Debug.Log($"The Level prerequisite for the quest is: {Quest.Prerequisites?.Items.Any()}");
            //Debug.Log($"The exp reward for the quest is: {Quest.Rewards.Exp}");
            //Debug.Log("[Compiler]" + Quest.SpecialEvent.EventID);


        }
        private void ProcessStartingNodeByPort(INode startingNode, string portName)
        {
            IPort port = startingNode.GetOutputPortByName(portName);
            if (port == null) return;

            if (!port.IsConnected)
            {
                Debug.LogWarning($"[QuestCompiler] Port: {portName}, is not connected to any porst");
                return;
            }

            List<IPort> connectedPorts = new();
            port.GetConnectedPorts(connectedPorts);

            foreach (var connectedPort in connectedPorts)
            {
                INode node = connectedPort.GetNode();
                if (node == null)
                {
                    Debug.LogWarning($"[QuestCompiler] Connected node on port: {portName}, is null.");
                    continue;
                }

                if (TryProcessNode(node))
                    proccessedNodes.Add(node.GetType());

            }
        }
        public void SaveQuestAsset()
        {
            if (Quest == null)
            {
                Debug.LogError("[QuestCompiler] Cannot save Quest coz its null. Make sure the graph compiled correctly.");
                return;
            }
            // path is Folder -> QuestName/Questname.asset
            string path = $"Assets/Resources/Quests/{Quest.ID}";
            //creates directory of path
            Directory.CreateDirectory(path);

            //combine path with .asset for asset creation
            path = Path.Combine(path, $"{Quest.ID}.asset");
            AssetDatabase.CreateAsset(Quest, path);

            // ping on project files
            Selection.activeObject = Quest;
            EditorGUIUtility.PingObject(Selection.activeObject);
        }
        public void SetGraph(QMGraph newGraph)
        {
            if (newGraph == null) return;
            ResetGraph();
            graph = newGraph;
        }
        public void ResetGraph()
        {
            if(graph == null) return;

            proccessedNodes.Clear();
            reg.Clear(true);
            Quest = null;
            startingNode = null;
        }
        private bool TryProcessNode(INode portOwnerNode)
        {

            if (!CanProccessNode(portOwnerNode))
                return false;

            if (IsHubNode(portOwnerNode, out QMBaseHubNode hub))
            {
                return ProccesHubNode(hub);
            }
            else if (IsContextNode(portOwnerNode, out QMBaseContextNode context))
            {
                return ProccessContextNode(context);

            }
            else Debug.Log($"{portOwnerNode.GetType().Name} Is not a valid node");

            return true;
        }

        private QuestSO BuildQuest()
        {
            if (reg.Modules == null || !reg.Modules.Any()) return null;
            if(reg.ObjectiveModules == null || !reg.ObjectiveModules.Any()) return null ;
            QuestSO quest = ScriptableObject.CreateInstance<QuestSO>();



            foreach (var module in reg.Modules)
            {
                module.Build(quest);
            }

            foreach (var module in reg.ObjectiveModules)
            {
                module.Build(quest);
            }

            return quest;
        }

        private bool ProccessContextNode(QMBaseContextNode node)
        {
            return node.ProcessNode(reg);
        }

        private INode LocateStartingNode(IEnumerable<INode> nodes)
        {
            INode startNode = nodes.FirstOrDefault(node => node is QMStartingNode)
                ?? throw new NullReferenceException($"[{GetType().Name}] Starting Node Is null");

            startingNode = startNode as QMStartingNode;

            return startNode;
        }

        private bool IsContextNode(INode node, out QMBaseContextNode contextNode)
        {
            if (node == null) throw new NullReferenceException($"[QuestCompiler].AsContextNode() node is null");
            contextNode = null;

            if (node is QMBaseContextNode)
            {
                contextNode = node as QMBaseContextNode;
                return true;
            }

            return false;
        }

        private bool IsHubNode(INode node, out QMBaseHubNode hubNode)
        {
            if (node == null) throw new NullReferenceException($"[QuestCompiler].AsHubNode() node is null");
            hubNode = null;

            if (node is QMBaseHubNode)
            {
                hubNode = node as QMBaseHubNode;
                return true;
            }

            return false;
        }
        private bool CanProccessNode(INode node)
        {
            Type type = node.GetType();
            if (proccessedNodes.Contains(type))
            {

                Debug.LogWarning($"Already proccessed a node of type {type}");
                return false;
            }
            Debug.Log($"Node of type {type} can be proccessed");
            return true;
        }

        private bool ProccesHubNode(INode node)
        {

            IPort hubOutputPort = node.GetOutputPortByName(QMBaseHubNode.HUB_OUTPUT_PORT);

            List<IPort> optionPorts = new();
            hubOutputPort.GetConnectedPorts(optionPorts);

            //TODO Move functionality to ContextNodes and just Build() on those;
            IEnumerable<IComposableNode> composableNodes = optionPorts
                .Select(port => port.GetNode())
                .OfType<IComposableNode>();

            if (!composableNodes.Any())
                return false;

            foreach (var composable in composableNodes)
            {
                //composable.Compose(cntx);
                Debug.Log($"Hub Composing {composable.GetType().Name}");
            }
            return true;
        }
    }
}

