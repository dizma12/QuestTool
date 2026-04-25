using UnityEngine;
using QuestMaker.Editor.Graph;
using QuestMaker.Editor.Nodes;
using System.Linq;
using System.Collections;
using NUnit.Framework;
using Unity.GraphToolkit.Editor;
using System.Collections.Generic;

namespace QuestMaker.Editor.Compiler
{
    
    public class QuestCompiler 
    {
        public QMGraph Graph { get; set; } = null;


        public QuestCompiler(QMGraph graph)
        {
            Graph = graph;
        }

        public void CompileQuestGraph()
        {
            var x = Graph.GetNodes();

            if (x.Count() <= 0) return;
            
            var startNode = x.FirstOrDefault(node => node is QMStartingNode);

            if(startNode != null)
            {
                Debug.Log($"Found starting node {startNode.GetType()}"); 
            }

           var prt = startNode.GetOutputPortByName(QMBaseNode.OUTPUT_PORT);
            List<IPort> connectedPorts = new();

            prt.GetConnectedPorts(connectedPorts);

            foreach( var port in connectedPorts )
            {
                INode portOwnerNode = port.GetNode();
                
                var p = portOwnerNode.GetType();
                Debug.Log(p);
                if(portOwnerNode.OutputPortCount > 0 )
                {
                    prt = portOwnerNode.GetOutputPortByName(QMBaseHubNode.HUB_OUTPUT_PORT);

                }
                
            }
        }
    }
}
