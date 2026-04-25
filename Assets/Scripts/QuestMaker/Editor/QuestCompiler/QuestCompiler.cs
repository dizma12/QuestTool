using NUnit.Framework;
using QuestMaker.Editor.Graph;
using QuestMaker.Editor.Nodes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace QuestMaker.Editor.Compiler
{

    public class QuestCompiler
    {
        public QMGraph Graph { get; set; } = null;
        private QuestCompilationContext context = new();

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
                if (portOwnerNode == null) Debug.Log("Node is null");
                else Debug.Log($"Found node of type {portOwnerNode.GetType()}");

                if (portOwnerNode is QMBaseHubNode hub)
                {
                    IPort hubOutputPort = hub.GetOutputPortByName(QMBaseHubNode.HUB_OUTPUT_PORT);

                    List<IPort> optionPorts = new();
                    hubOutputPort.GetConnectedPorts(optionPorts);

                    IEnumerable<IComposableNode> composableNodes = optionPorts
                        .Select(n => n.GetNode())
                        .OfType<IComposableNode>();

                    foreach (var composable in composableNodes) 
                    {
                        composable.Compose(context);
                    }

                }

                //var p = portOwnerNode.GetType();
                //Debug.Log(p);
                //if(portOwnerNode.OutputPortCount > 0 )
                //{
                //    prt = portOwnerNode.GetOutputPortByName(QMBaseHubNode.HUB_OUTPUT_PORT);

                //}

            }
        }
        private INode LocateStartingNode(IEnumerable<INode> nodes)
        {
            INode startNode = nodes.FirstOrDefault(node => node is QMStartingNode);

            if (startNode != null)
                Debug.Log($"Found starting node {startNode.GetType()}");
            
            return startNode;
        }
    }
}

