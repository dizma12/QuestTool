using QuestMaker.Editor.Compiler.CompilationModules;
using QuestMaker.Editor.Graph;
using QuestMaker.Editor.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace QuestMaker.Editor.Compiler
{

    public class QuestCompiler
    {
        public QMGraph Graph { get; set; } = null;
        private QuestCompilationContext context = new();
        private QuestModuleContext cntx = new();
        private HashSet<Type> proccessedHubs = new();
        public QuestCompiler(QMGraph graph)
        {
            Graph = graph;
        }

        public void CompileQuestGraph()
        {
            IEnumerable<INode> graphNodes = Graph.GetNodes();

            if (graphNodes.Count() <= 0) return;

            INode startNode = LocateStartingNode(graphNodes);

            IPort flowPort = startNode.GetOutputPortByName(QMBaseNode.OUTPUT_PORT);

            List<IPort> connectedPorts = new();
            flowPort.GetConnectedPorts(connectedPorts);
            Debug.Log($"Connected ports: {connectedPorts.Count}");

            foreach (var port in connectedPorts)
            {
                INode portOwnerNode = port.GetNode();
                Type portOwnerNodeType = portOwnerNode.GetType();
                if (portOwnerNode == null) Debug.Log("Node is null");
                else Debug.Log($"Found node of type {portOwnerNodeType}");


                    if(CanProccessTypeOfHubNode(portOwnerNode))
                        proccessedHubs.Add(portOwnerNodeType);
                    else return;

                    //var interfaceType = QMGraphUtility.GetTypeFromGenericInterface(portOwnerNodeType, typeof(IHubNodeCollector<>));

                    //Debug.Log($"The type of interace is: {interfaceType}");

                    IPort hubOutputPort = portOwnerNode.GetOutputPortByName(QMBaseHubNode.HUB_OUTPUT_PORT);

                    List<IPort> optionPorts = new();
                    hubOutputPort.GetConnectedPorts(optionPorts);

                    IEnumerable<IComposableNode> composableNodes = optionPorts
                        .Select(n => n.GetNode())
                        .OfType<IComposableNode>();

                    foreach (var composable in composableNodes)
                    {
                        composable.Compose(cntx);
                    }

                //var p = portOwnerNode.GetType();
                //Debug.Log(p);
                //if(portOwnerNode.OutputPortCount > 0 )
                //{
                //    prt = portOwnerNode.GetOutputPortByName(QMBaseHubNode.HUB_OUTPUT_PORT);

                //}

            }

            var q = cntx.Build();


            Debug.Log($"Level Prereq= {q.LevelPrereq}");

            if(q.ItemPrereq != null)
                Debug.Log($"Item Prereq= {q.ItemPrereq.First().ItemName}");

        }
        private INode LocateStartingNode(IEnumerable<INode> nodes)
        {
            INode startNode = nodes.FirstOrDefault(node => node is QMStartingNode);

            if (startNode != null)
                Debug.Log($"Found starting node {startNode.GetType()}");

            return startNode;
        }

        private bool CanProccessTypeOfHubNode(INode hub)
        {
            Type hubType = hub.GetType();
            if(hub is not QMBaseHubNode)
            {
                Debug.LogWarning($"Invalid type {hubType}");
                return false;
            }
            if(proccessedHubs.Contains(hubType))
            {

                Debug.LogWarning($"Already proccessed a hub of type {hubType}");
                return false;
            }
            Debug.LogWarning($"Proccessing hub of type {hubType}");
            return true;
        }
    }
}

