using QuestMaker.Data;
using QuestMaker.Editor.Compiler.CompilationModules;
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

        public void CompileQuestGraph()
        {
            IEnumerable<INode> graphNodes = graph.GetNodes();

            if (graphNodes.Count() <= 0) return;

            INode startNode = LocateStartingNode(graphNodes);

            IPort flowPort = startNode.GetOutputPortByName(QMBaseOptionNode.OUTPUT_PORT);

            List<IPort> connectedPorts = new();
            flowPort.GetConnectedPorts(connectedPorts);

            if (!connectedPorts.Any()) return;



            Debug.Log($"Connected ports: {connectedPorts.Count}");

            foreach (var port in connectedPorts)
            {
                INode portOwnerNode = port.GetNode();
                Type portOwnerNodeType = portOwnerNode.GetType();
                if (portOwnerNode == null) Debug.Log("Node is null");
                else Debug.Log($"Found node of type {portOwnerNodeType}");

                if (TryProcessNode(portOwnerNode))
                    proccessedNodes.Add(portOwnerNode.GetType());

            }
            Quest = BuildQuest();

            QMStartingNode start = (QMStartingNode)startNode;
            start.Build(Quest);

            Debug.Log($"The name of the quest is: {Quest.QuestName}");
            Debug.Log($"The Level prerequisite for the quest is: {Quest.Prerequisites.Level}");
            Debug.Log($"The exp reward for the quest is: {Quest.Rewards.Exp}");
            Debug.Log($"The type of the first step is: {Quest.Objectives.First().Steps.First().StepType}");

        }
        public void SaveQuestAsset()
        {
            // path is Folder -> QuestName/Questname.asset
            string path = $"Assets/Resources/Quests/{Quest.QuestName}";
            //creates directory of path
            Directory.CreateDirectory(path);

            //combine path with .asset for asset creation
            path = Path.Combine(path, $"{Quest.QuestName}.asset");
            AssetDatabase.CreateAsset(Quest, path);

            // ping on project files
            Selection.activeObject = Quest;
            EditorGUIUtility.PingObject(Selection.activeObject);
        }
        public void SetGraph(QMGraph newGraph)
        {
            proccessedNodes.Clear();
            reg.Clear(true);
            Quest = null;
            graph = newGraph;
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
            if (!reg.Modules.Any()) return null;

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

        private bool ProccessContextNode(QMBaseContextNode contextNode)
        {
            return contextNode.ProccessNodes(reg);
        }

        private INode LocateStartingNode(IEnumerable<INode> nodes)
        {
            INode startNode = nodes.FirstOrDefault(node => node is QMStartingNode)
                ?? throw new NullReferenceException($"[{GetType().Name}] Starting Node Is null");

            Debug.Log($"Found starting node {startNode.GetType()}");

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

