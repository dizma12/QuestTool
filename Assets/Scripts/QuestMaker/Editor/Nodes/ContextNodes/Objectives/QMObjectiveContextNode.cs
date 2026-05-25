
using QuestMaker.Editor.Compiler;
using QuestMaker.Editor.Compiler.CompilationModules;
using System;
using System.Collections.Generic;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace QuestMaker.Editor.Nodes.ContextNodes
{
    [Serializable]
    internal class QMObjectiveContextNode : QMBaseContextNode
    {
        public const string SPECIAL_EVENT_PORT = "Special_Event_Port";
        public const string OBJECTIVE_FLOW_PORT = "OBJECTIVE_FLOW_PORT";
        public override bool AllowMultipleContextNodesOfSameType => true;

        public override Type PortType => typeof(QMFlowHelper);
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {

            context.AddOutputPort(OBJECTIVE_FLOW_PORT)
                   .WithDataType(typeof(ISpecialEventNode))
                   .WithDisplayName("Objective Flow")
                   .WithConnectorUI(PortConnectorUI.Arrowhead)
                   .Build();

            context.AddOutputPort(SPECIAL_EVENT_PORT)
                   .WithDataType(typeof(ISpecialEventNode))
                   .WithDisplayName("Special Event")
                   .WithConnectorUI(PortConnectorUI.Circle)
                   .Build();


            base.OnDefinePorts(context);
        }
        public override bool ProccessNodes(ModuleBuilderRegistry reg)
        {

            IComposableNode[] nodes = GetBlockNodes();
            if (nodes == null || nodes.Length == 0)
                return false;

            ObjectiveModule module = reg.RequestNewObjectiveModule<ObjectiveModule>();


            foreach (var block in nodes)
            {
                block.Compose(module, reg);
            }

            return true;
        }

        private void CheckForSpecialEvents()
        {
            if (!GetOutputPortByName(SPECIAL_EVENT_PORT).IsConnected) return;

            List<IPort> connected = new();

            GetOutputPortByName(SPECIAL_EVENT_PORT).GetConnectedPorts(connected);
            if (connected.Count > 1)
            {
                Debug.LogError("You cant have more than 1 special event to objective nodes");
                return;
            }

            foreach (var port in connected)
            {
                Debug.Log("[QMOBjectiveNode Not Implemented]!!!");
            }

        }
    }
}
